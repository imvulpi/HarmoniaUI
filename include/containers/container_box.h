#pragma once

#include <godot_cpp/godot.hpp>
#include <godot_cpp/core/class_db.hpp>
#include <godot_cpp/classes/control.hpp>
#include <godot_cpp/classes/input_event.hpp>
#include <godot_cpp/variant/typed_array.hpp>
#include <godot_cpp/classes/v_scroll_bar.hpp>
#include <godot_cpp/classes/h_scroll_bar.hpp>
#include "commons/container_unit_converter.h"
#include "core/systems/alert/alert_manager.h"
#include "core/harmonia.h"
#include "containers/content_box.h"

using namespace godot;

/// @brief Container class for all HarmoniaUI containers
class ContainerBox : public Control
{
    GDCLASS(ContainerBox, Control);
public:
    ContainerBox() = default;
    ~ContainerBox() = default;
        
    /// @brief Alert of layout change, that being position, width, height or other layout change
    const String ALERT_LAYOUT_CHANGE = "layout-change";

    /// @brief Update time from the last update
    /// @note This will get removed in the future.
    double update_time {0};

    /// @brief The interval of updates 
    /// @note This will get removed in the future.
    double update_interval {1.0};

    void on_window_size_changed();
    void _ready();
    void _process(double delta);

    Harmonia::Visibility visibility {Harmonia::Visibility::OBJECT_VISIBLE};
    void set_visibility(Harmonia::Visibility new_visibility);
    Harmonia::Visibility get_visibility();
    void apply_visibility();

    Size2 window_size;

    /// @brief Content box of this container if it's using one.
    ContentBox* content_box {nullptr};
    /// @brief Attempts to find a content box inside a container.
    /// @return A content box associated with the container.
    ContentBox* find_content_box();
    /// @brief Creates a content box, sets few stuff in the contentbox
    void create_content_box();

    /// @brief Calculates overflow from provided same axis container size, check size and current overflow
    /// @param container Size of the container
    /// @param check_size Size of the checking size
    /// @param current_overflow The current overflow
    /// @return Overflow or current overflow if calculated is not bigger
    double calculate_overflow(double container, double check_size, double current_overflow);

    /// @brief Checks overflows Y and X and sets overflow values.
    /// @param overflow the overflows
    void check_overflows(Vector2 overflow);

    /// @brief Updates this containers overflows calculated from children and other
    /// @param children the children nodes of this container
    void update_container_overflows(TypedArray<Node> children);

    /// @brief Negative oveflow on X axis specifically LEFT (in px)
    /// @note this is counted towards the normal overflow size and is applied to scrolls to move them according to overflow
    double negative_overflow_x {0};

    /// @brief Negative overflow on Y axis specifically TOP (in px)
    /// @note This is counted towards the normal overflow size and is applied to scrolls to move them according to overflow
    double negative_overflow_y {0};

    /// @brief Is X axis overflowed
    bool is_overflowed_x { false };
    /// @brief Size of the X overflowing
    LengthPair overflow_x_size;
    /// @brief Sets size of X overflowing
    /// @param value size of overflowing
    /// @param unit_type unit of the size
    void set_overflow_x_size(double value, Harmonia::Unit unit_type = Harmonia::PIXEL);
    /// @brief Gets size of X overflowing
    /// @param unit_type unit which it should be returned in
    double get_overflow_x_size(Harmonia::Unit unit_type = Harmonia::PIXEL);

    /// @brief is Y axis overflowed
    bool is_overflowed_y { false };
    /// @brief Size of the Y overflowing
    LengthPair overflow_y_size;
    /// @brief Sets size of Y overflowing
    /// @param value size of overflowing
    /// @param unit_type unit of the size
    void set_overflow_y_size(double value, Harmonia::Unit unit_type = Harmonia::PIXEL);
    /// @brief Gets a size of Y size overflowing
    /// @param unit_type unit which you want overflow y size to be returned in
    double get_overflow_y_size(Harmonia::Unit unit_type = Harmonia::PIXEL);

    /// @brief Updates positions of vertical and horizontal scrollbars, for now: vertical on left, horizontal down
    void position_scrolls();

    /// @brief Updates scrolls based on overflows etc.
    void update_scrolls();

    /// @brief Vertical scroll which will be used when overflowing occurs
    VScrollBar* vertical_scroll {nullptr};
    /// @brief Path to vertical scroll, editor way of setting vertical scroll.
    NodePath vertical_scroll_path {""};
    /// @brief Simple setter for vertical scroll in editor
    /// @param scroll the path to vertical scroll.
    void set_vertical_scroll(NodePath scroll);
    /// @brief Simple getter for vertical scroll in editor
    NodePath get_vertical_scroll();

    /// @brief Horizontal scroll which will be used when overflowing occurs
    HScrollBar* horizontal_scroll {nullptr};
    /// @brief Path to horizontal scroll, editor way of setting horizontal scroll.
    NodePath horizontal_scroll_path {""};
    /// @brief Simple setter for horizontal scroll in editor
    /// @param scroll the path to horizontal scroll.
    void set_horizontal_scroll(NodePath scroll);
    /// @brief Simple getter for horizontal scroll in editor
    NodePath get_horizontal_scroll();

    /// @brief Value in which scroll down or up will change when scrolling is detected
    LengthPair scroll_y_step;
    /// @brief A way of setting step in the editor, step y
    String string_scroll_y_step;
    /// @brief Simple string setter for step y
    /// @param value step y
    void set_string_scroll_y_step(String value);
    /// @brief Simple string getter for step y
    String get_string_scroll_y_step();
    /// @brief Simple setter for step y
    /// @param value step y
    /// @param unit_type unit of value 
    void set_scroll_y_step(double value, Harmonia::Unit unit_type = Harmonia::PIXEL);
    /// @brief Simple getter for step y
    /// @param value step y
    /// @param unit_type unit of value 
    double get_scroll_y_step(Harmonia::Unit unit_type = Harmonia::PIXEL);
   
    /// @brief Value in which scroll left or right will change when scrolling is detected
    LengthPair scroll_x_step;
    /// @brief A way of setting step in the editor, Value in which scroll left or right will change when scrolling is detected
    String string_scroll_x_step;
    /// @brief Simple string setter for step x
    /// @param value step x
    void set_string_scroll_x_step(String value);
    /// @brief Simple string getter for step x
    String get_string_scroll_x_step();
    /// @brief Simple setter for step x
    /// @param value step x
    void set_scroll_x_step(double value, Harmonia::Unit unit_type = Harmonia::PIXEL);
    /// @brief Simple getter for step x
    /// @param value step x
    double get_scroll_x_step(Harmonia::Unit unit_type = Harmonia::PIXEL);

    /// @brief Functions as _draw but doesnt override the _draw instead it's used when draw notification is emmited.  
    void draw_ui();

    /// @brief Helper function for getting width of a provided LengthPair in any Harmonia unit. (Here to remove repetitions)
    /// @param pair The LengthPair you want the width from.
    /// @param unit_type Harmonia unit you want the width to be in.
    /// @return Width in the specified unit you want.
    double get_width_length_pair_unit(LengthPair pair, Harmonia::Unit unit_type);

    /// @brief Helper function for getting height of a provided LengthPair in any Harmonia unit. (Here to remove repetitions)
    /// @param pair The LengthPair you want the height from.
    /// @param unit_type Harmonia unit you want the height to be in.
    /// @return Height in the specified unit you want.
    double get_height_length_pair_unit(LengthPair pair, Harmonia::Unit unit_type);

    /// @brief Updates the container presentation/view in runtime
    void update_presentation();

    /// @brief Self positioning, gets called when there is no parent
    void update_self_position();

    /// @brief Updates the container children positions in runtime
    void update_children_position(TypedArray<Node> children);

    /// @brief Updates a child control node anchors based on this container, for example enforces min/max values for anchors
    /// @param control The child control taht anchors should be updated on.
    void update_control_anchors(Control* control);

    /// @brief Alert manager of this containers. Bind to this manager if you want to react to this containers alerts
    AlertManager* alert_manager = nullptr;
    /// @brief A simple getter for alert manager of this container
    AlertManager* get_alert_manager();
    /// @brief A simple setter for alert manager of this container
    void set_alert_manager(AlertManager* manager);

    /// @brief ContainerBox parent of this container, only ContainerBox classes are set.
    ContainerBox* parent = nullptr;

    /// @brief gets containerBox parent of this container if one exists.
    /// @return ContainerBox parent or nullptr
    ContainerBox* get_parent_container();

    /// NOTE: Padding and Margins go: [0: up] [1: right] [2: down] [3: left]

    /// @brief paddings in string which later gets processed to retrieve up/down/left/right paddings. 
    String padding_str;
    /// @brief Setter for string padding, processes the new padding in order to retrieve actual paddings.
    void set_padding_str(String new_padding);
    /// @brief Simple getter for string padding. 
    String get_padding_str();

    /// @brief A helper function to set all paddings with the same value and unit.
    /// @param all_sides value of each side.
    /// @param unit_type unit type of each side.
    void set_padding_all(double all_sides, Harmonia::Unit unit_type = Harmonia::PIXEL, bool dispatch_alert_and_queue = true);
    /// @brief A helper function to set paddings on the y/vertical axis (specifically down/up)
    /// @param vertical_y value of each y/vertical sides (down/up)
    /// @param vertical_unit unit type of each y/vertical sides (down/up)
    void set_padding_y_vertical(double vertical_y, Harmonia::Unit vertical_unit = Harmonia::PIXEL, bool dispatch_alert_and_queue = true);
    
    /// @brief A helper function to set paddings on the x/horizontal axis (specifically left/right)
    /// @param horizontal_x value of each x/horizontal sides (left/right)
    /// @param horizontal_unit unit type of each x/horizontal sides (left/right)
    void set_padding_x_horizontal(double horizontal_x, Harmonia::Unit horizontal_unit = Harmonia::PIXEL, bool dispatch_alert_and_queue = true);

    /// @brief A helper function to get all paddings as a typed array. 
    /// 
    /// indexes values are: 0-up, 1-right, 2-down, 3-left 
    /// @param unit_type what unit type should the paddings be in.
    TypedArray<double> get_paddings(Harmonia::Unit unit_type = Harmonia::PIXEL);

    /// @brief Upper(Up) padding of this container
    LengthPair padding_up;
    /// @brief Simple setter for the upper(Up) padding, dispatches an alert.
    /// @param dispatch_alert_and_queue Whether an alert for padding change should be dispatched and draw queued.
    void set_padding_up(double up, Harmonia::Unit up_unit, bool dispatch_alert_and_queue = true);
    /// @brief Simple getter for the upper(Up) padding in any harmonia unit
    double get_padding_up(Harmonia::Unit unit_type = Harmonia::Unit::PIXEL);
    
    /// @brief Lower(Down) padding of this container
    LengthPair padding_down;
    /// @brief Simple setter for the lower(Down) padding, dispatches an alert.
    /// @param dispatch_alert_and_queue Whether an alert for padding change should be dispatched and draw queued.
    void set_padding_down(double down, Harmonia::Unit down_unit, bool dispatch_alert_and_queue = true);
    /// @brief Simple getter for the lower(Down) padding in any harmonia unit
    double get_padding_down(Harmonia::Unit unit_type = Harmonia::Unit::PIXEL);

    /// @brief Left padding of this container
    LengthPair padding_left;
    /// @brief Simple setter for the left padding, dispatches an alert.
    /// @param dispatch_alert_and_queue Whether an alert for padding change should be dispatched and draw queued.
    void set_padding_left(double left, Harmonia::Unit left_unit, bool dispatch_alert_and_queue = true);
    /// @brief Simple getter for the left padding in any harmonia unit
    double get_padding_left(Harmonia::Unit unit_type = Harmonia::Unit::PIXEL);
   
    /// @brief Right padding of this container
    LengthPair padding_right;
    /// @brief Simple setter for the right padding, dispatches an alert.
    /// @param dispatch_alert_and_queue Whether an alert for padding change should be dispatched and draw queued.
    void set_padding_right(double right, Harmonia::Unit right_unit, bool dispatch_alert_and_queue = true);
    /// @brief Simple getter for the right padding in any harmonia unit
    double get_padding_right(Harmonia::Unit unit_type = Harmonia::Unit::PIXEL);

    /// @brief Margins in string which later gets processed to retrieve up/down/left/right margins. 
    String margin_str;
    /// @brief Setter for string margin, processes the new margin in order to retrieve actual margins.
    void set_margin_str(String new_margin);
    /// @brief Simple getter for string margin. 
    String get_margin_str();

    /// @brief A helper function to set all margins with the same value and unit.
    /// @param all_sides value of each side.
    /// @param unit_type unit type of each side.
    void set_margin_all(double all_sides, Harmonia::Unit unit_type = Harmonia::PIXEL, bool dispatch_alert = true);
    /// @brief A helper function to set margins on the y/vertical axis (specifically down/up)
    /// @param vertical_y value of each y/vertical sides (down/up)
    /// @param vertical_unit unit type of each y/vertical sides (down/up)
    void set_margin_y_vertical(double vertical_y, Harmonia::Unit vertical_unit = Harmonia::PIXEL, bool dispatch_alert = true);
    
    /// @brief A helper function to set margins on the x/horizontal axis (specifically left/right)
    /// @param horizontal_x value of each x/horizontal sides (left/right)
    /// @param horizontal_unit unit type of each x/horizontal sides (left/right)
    void set_margin_x_horizontal(double horizontal_x, Harmonia::Unit horizontal_unit = Harmonia::PIXEL, bool dispatch_alert = true);

    /// @brief A helper function to get all margins as a typed array. 
    /// 
    /// indexes values are: 0-up, 1-right, 2-down, 3-left 
    /// @param unit_type what unit type should the margins be in.
    TypedArray<double> get_margins(Harmonia::Unit unit_type = Harmonia::PIXEL);

    /// @brief Upper(Up) margin of this container
    LengthPair margin_up;
    /// @brief Simple setter for the upper(Up) margin, dispatches an alert.
    /// @param dispatch_alert Whether an alert for margin change should be dispatched
    void set_margin_up(double up, Harmonia::Unit up_unit, bool dispatch_alert = true);
    /// @brief Simple getter for the upper(Up) margin in any harmonia unit
    double get_margin_up(Harmonia::Unit unit_type = Harmonia::Unit::PIXEL);
    
    /// @brief Lower(Down) margin of this container
    LengthPair margin_down;
    /// @brief Simple setter for the lower(Down) margin, dispatches an alert.
    /// @param dispatch_alert Whether an alert for margin change should be dispatched
    void set_margin_down(double down, Harmonia::Unit down_unit, bool dispatch_alert = true);
    /// @brief Simple getter for the lower(Down) margin in any harmonia unit
    double get_margin_down(Harmonia::Unit unit_type = Harmonia::Unit::PIXEL);

    /// @brief Left margin of this container
    LengthPair margin_left;
    /// @brief Simple setter for the left margin, dispatches an alert.
    /// @param dispatch_alert Whether an alert for margin change should be dispatched
    void set_margin_left(double left, Harmonia::Unit left_unit, bool dispatch_alert = true);
    /// @brief Simple getter for the left margin in any harmonia unit
    double get_margin_left(Harmonia::Unit unit_type = Harmonia::Unit::PIXEL);
   
    /// @brief Right margin of this container
    LengthPair margin_right;
    /// @brief Simple setter for the right margin, dispatches an alert.
    /// @param dispatch_alert Whether an alert for margin change should be dispatched
    void set_margin_right(double right, Harmonia::Unit right_unit, bool dispatch_alert = true);
    /// @brief Simple getter for the right margin in any harmonia unit
    double get_margin_right(Harmonia::Unit unit_type = Harmonia::Unit::PIXEL);

    /// @brief Background color of this container, by default transparent.
    Color background_color = Color(0, 0, 0, 0);
    /// @brief Simple getter for the background color of this container
    Color get_background_color();
    /// @brief Simple setter for the background color of this container. Additionally calls for redraw so setting background color has a close to immediate effect.
    void set_background_color(Color color);

    /// @brief The type of positioning of this containers children. Default STATIC.
    Harmonia::Position position_type {Harmonia::Position::STATIC};

    /// @brief Sets a positioning type for this containers children.
    void set_position_type(Harmonia::Position new_type);
    /// @brief Returns the positioning type for this containers children
    Harmonia::Position get_position_type();
    
    Harmonia::OverflowBehaviour overflow_behaviour {Harmonia::SCROLL};

    /// @brief Setter for an overflowing behaviour of xy axes
    void set_overflow_behaviour(Harmonia::OverflowBehaviour behaviour);

    /// @brief Getter for an overflowing behaviour of xy axes
    Harmonia::OverflowBehaviour get_overflow_behaviour();

    /// NOTE: BELOW Str pos are positions set in the editor or in the code using getter/setter
    /// The string positions get processed to create a pos_x length pair. Ex of str pos: 10%, 10px

    /// @brief X Position of this container.
    LengthPair pos_x;

    /// @brief X position setter in harmonia units.
    void set_pos_x(double new_x, Harmonia::Unit unit_type = Harmonia::Unit::PIXEL);

    /// @brief Retrieves X position in a provided unit.
    /// @param unit_type The unit type you want x pos to be returned in.
    double get_pos_x(Harmonia::Unit unit_type = Harmonia::Unit::PIXEL);

    /// @brief String posistion that gets processed to create a pos_x length pair when set by setter method.
    String pos_x_str;

    /// @brief Setter for string position X in harmonia units.
    void set_pos_x_str(String new_x);

    /// @brief Getter for string position X, includes harmonia unit.
    /// @note return ex: '100px', '100%', '100vw', '100vh'
    String get_pos_x_str();

    /// @brief Y Position of this container.
    LengthPair pos_y;

    /// @brief Y position setter in harmonia units.
    void set_pos_y(double new_y, Harmonia::Unit unit_type = Harmonia::Unit::PIXEL);

    /// @brief Retrieves Y position in a provided unit.
    /// @param unit_type The unit type you want y pos to be returned in.
    double get_pos_y(Harmonia::Unit unit_type = Harmonia::Unit::PIXEL);

    /// @brief String posistion that gets processed to create a pos_y length pair when set by setter method.
    String pos_y_str;

    /// @brief Setter for string position X in harmonia units.
    void set_pos_y_str(String new_y);

    /// @brief Getter for string position X, includes harmonia unit.
    /// @note return ex: '100px', '100%', '100vw', '100vh'
    String get_pos_y_str();
    
    /// @brief Should debug outputs be printed to the console?
    bool debug_outputs = false;
    
    /// @brief Sets debug outputs
    /// @param debug_outputs Should debug outputs be printed?
    void set_debug_outputs(bool debug_outputs);

    /// @brief Gets the debug outputs setting
    /// @return debug outputs option
    bool get_debug_outputs();

    LengthPair width; /* Width pair, stores length and unit */

    /// @brief Gets the width in provided unit
    /// @param unit_type Width unit type
    /// @return Width in the provided unit type
    double get_width(Harmonia::Unit unit_type = Harmonia::Unit::PIXEL);

    /// @brief Calculates the width with addition of other specific attributes, whether thats container specific or for example padding.
    /// @param unit_type The unit type total width should be returned with.
    double calculate_total_width(Harmonia::Unit unit_type = Harmonia::Unit::PIXEL);

    /// @brief Sets the width to provided length and unit
    /// @param length Width length
    /// @param unit_type Width length unit type 
    void set_width(double length, Harmonia::Unit unit_type = Harmonia::Unit::PIXEL);

    /// @brief Gets current width pair in a string representation
    /// @return Width string representation
    String get_width_str();

    /// @brief Sets width from a string representation of width length pair 
    /// @param length_and_unit string representataion of width length pair
    void set_width_str(String length_and_unit);
    String width_str; /* String representation of width */
    
    LengthPair height; /* Height pair, stores length and unit */

    /// @brief Gets the height in provided unit
    /// @param unit_type height unit type
    /// @return Height in the provided unit type
    double get_height(Harmonia::Unit unit_type = Harmonia::Unit::PIXEL);

    /// @brief Calculates the height with addition of other specific attributes, whether thats container specific or for example padding.
    /// @param unit_type The unit type total height should be returned with.
    double calculate_total_height(Harmonia::Unit unit_type = Harmonia::Unit::PIXEL);

    /// @brief Sets the height to provided length and unit
    /// @param length Height length
    /// @param unit_type Height length unit type 
    void set_height(double length, Harmonia::Unit unit_type = Harmonia::Unit::PIXEL);
    
    /// @brief Gets current height pair in a string representation
    /// @return Height string representation
    String get_height_str();

    /// @brief Sets height from a string representation of height length pair 
    /// @param length_and_unit string representataion of height length pair
    void set_height_str(String length_and_unit);
    String height_str; /* String representation of height */

protected:
    static void _bind_methods();
	bool _set(const StringName &p_name, const Variant &p_value);
	bool _get(const StringName &p_name, Variant &r_ret) const;
    void _get_property_list(List<PropertyInfo> *p_list) const;
    void _notification(int p_what);
};