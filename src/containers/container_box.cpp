#include "containers/container_box.h"
#include <godot_cpp/classes/engine.hpp>
#include <godot_cpp/variant/utility_functions.hpp>
#include <godot_cpp/classes/scene_tree.hpp>
#include <godot_cpp/classes/window.hpp>
#include <godot_cpp/classes/project_settings.hpp>
#include <godot_cpp/classes/input_event_mouse_button.hpp>
#include <godot_cpp/classes/rendering_server.hpp>
#include "core/systems/alert/layout/alert_layout_change.h"
#include "commons/string_helper.h"

void ContainerBox::_enter_tree(){
    if (Engine::get_singleton()->is_editor_hint()) {
        window_size = Size2(
            ProjectSettings::get_singleton()->get_setting("display/window/size/viewport_width"), 
            ProjectSettings::get_singleton()->get_setting("display/window/size/viewport_height"));

        ContentBox* found_content_box = find_content_box();
        if(found_content_box == nullptr){
            create_content_box();
        }else{
            content_box = found_content_box;
        }
    }else{
        get_tree()->get_root()->connect("size_changed", Callable(this, "on_window_size_changed"));
        window_size = get_tree()->get_root()->get_visible_rect().size;
    }
}

void ContainerBox::on_window_size_changed(){
    window_size = get_tree()->get_root()->get_visible_rect().size;
}

void ContainerBox::_ready(){
    set_process(true);
    set_string_scroll_y_step("10px");
    set_string_scroll_x_step("10px");
    if(content_box){
        content_box->overflowing_behaviour = overflow_behaviour;
    }else{
        ContentBox* found_content_box = find_content_box();
        if(found_content_box == nullptr){
        }else{
            content_box = found_content_box;
        }
    }

    Node* vnode = get_node_or_null(vertical_scroll_path);
    if(auto* vscroll = Object::cast_to<VScrollBar>(vnode)){
        vertical_scroll = vscroll;
        if(content_box){
            content_box->scrollbar_y = vscroll;
        }
    }

    Node* hnode = get_node_or_null(horizontal_scroll_path);
    if(auto* hscroll = Object::cast_to<HScrollBar>(hnode)){
        horizontal_scroll = hscroll;
        if(content_box){
            content_box->scrollbar_x = hscroll;
        }
    }
}

void ContainerBox::_process(double time) {
    update_time += time;
    if(update_time >= update_interval){
        update_presentation();
        update_time -= update_interval;
    }
}

ContentBox* ContainerBox::find_content_box(){
    TypedArray<Node> nodes = get_children();

    for (size_t i = 0; i < nodes.size(); i++)
    {
        if(auto* content_box = Object::cast_to<ContentBox>(nodes[i])){
            return content_box;
        }
    }
    
    return nullptr;
}

void ContainerBox::create_content_box(){
    ContentBox* new_content_box = memnew(ContentBox());
    content_box = new_content_box;
    new_content_box->set_name("Content");
    add_child(new_content_box);
    new_content_box->set_owner(get_tree()->get_edited_scene_root());
}

double ContainerBox::calculate_overflow(double container, double check_size, double current_overflow){
    if(check_size > container){
        double overflow = check_size - container;
        if(overflow > current_overflow){
            return overflow;
        }
    }

    return current_overflow;
}

double ContainerBox::get_overflow_width_length_pair_unit(LengthPair pair, Harmonia::Unit unit_type){
    if(unit_type == Harmonia::Unit::NOT_SET) return 0;

    if(parent == nullptr){
        return ContainerUnitConverter::get_width(pair, window_size.x, window_size, unit_type);
    }

    return ContainerUnitConverter::get_width(pair, parent->get_width(), window_size, unit_type);
}

double ContainerBox::get_overflow_height_length_pair_unit(LengthPair pair, Harmonia::Unit unit_type){
    if(unit_type == Harmonia::Unit::NOT_SET) return 0;

    if(parent == nullptr){
        return ContainerUnitConverter::get_height(pair, window_size.y, window_size, unit_type);
    }

    return ContainerUnitConverter::get_height(pair, parent->get_height(), window_size, unit_type);
}

void ContainerBox::set_overflow_x_size(double value, Harmonia::Unit unit_type){
    overflow_x_size.length = value;
    overflow_x_size.unit_type = unit_type;
}

double ContainerBox::get_overflow_x_size(Harmonia::Unit unit_type){
    return get_overflow_width_length_pair_unit(overflow_x_size, unit_type);
}

void ContainerBox::set_overflow_y_size(double value, Harmonia::Unit unit_type){
    overflow_y_size.length = value;
    overflow_y_size.unit_type = unit_type;
}

double ContainerBox::get_overflow_y_size(Harmonia::Unit unit_type){
    return get_overflow_height_length_pair_unit(overflow_y_size, unit_type);
}

void ContainerBox::position_scrolls(){
    Vector2 vscroll_size = Vector2(0,0);
    if(vertical_scroll){
        vscroll_size = vertical_scroll->get_size();
        vertical_scroll->set_size(Vector2(vscroll_size.x, get_height()));
        vertical_scroll->set_position(Vector2(get_width()-vscroll_size.x, 0));
    } 

    if(horizontal_scroll){
        Vector2 hscroll_size = horizontal_scroll->get_size();
        if(vertical_scroll && vertical_scroll->is_visible()){
            horizontal_scroll->set_size(Vector2(get_width()-vscroll_size.x, hscroll_size.y));
        }else{
            horizontal_scroll->set_size(Vector2(get_width(), hscroll_size.y));
        }
        horizontal_scroll->set_position(Vector2(0, get_height()-hscroll_size.y));
    }
}

void ContainerBox::set_vertical_scroll(NodePath scroll){
    vertical_scroll_path = scroll;
    Node* node = get_node_or_null(scroll);
    if(auto* vscroll = Object::cast_to<VScrollBar>(node)){
        vertical_scroll = vscroll;
        if(content_box){
            content_box->scrollbar_y = vscroll;
        }
    }
}

NodePath ContainerBox::get_vertical_scroll(){
    return vertical_scroll_path;
}

void ContainerBox::set_horizontal_scroll(NodePath scroll){
    horizontal_scroll_path = scroll;
    Node* node = get_node_or_null(scroll);
    if(auto* hscroll = Object::cast_to<HScrollBar>(node)){
        horizontal_scroll = hscroll;
        if(content_box){
            content_box->scrollbar_x = hscroll;
        }
    }
}

NodePath ContainerBox::get_horizontal_scroll(){
    return horizontal_scroll_path;
}

void ContainerBox::set_string_scroll_y_step(String value){
    string_scroll_y_step = value;
    LengthPair new_scroll_y_step = LengthPair::get_pair(value);
    set_scroll_y_step(new_scroll_y_step.length, new_scroll_y_step.unit_type);
}

String ContainerBox::get_string_scroll_y_step(){
    return string_scroll_y_step;
}

void ContainerBox::set_scroll_y_step(double value, Harmonia::Unit unit_type){
    scroll_y_step.length = value;
    scroll_y_step.unit_type = unit_type;
    if(content_box){
        content_box->scroll_step_top_px = get_scroll_y_step();
    }
}

double ContainerBox::get_scroll_y_step(Harmonia::Unit unit_type){
    return get_overflow_height_length_pair_unit(scroll_y_step, unit_type);
}

void ContainerBox::set_string_scroll_x_step(String value){
    string_scroll_x_step = value;
    LengthPair new_scroll_x_step = LengthPair::get_pair(value);
    set_scroll_x_step(new_scroll_x_step.length, new_scroll_x_step.unit_type);
    if(content_box){
        content_box->scroll_step_left_px = get_scroll_x_step();
    }
}

String ContainerBox::get_string_scroll_x_step(){
    return string_scroll_x_step;
}

void ContainerBox::set_scroll_x_step(double value, Harmonia::Unit unit_type){
    scroll_x_step.length = value;
    scroll_x_step.unit_type = unit_type;
}

double ContainerBox::get_scroll_x_step(Harmonia::Unit unit_type){
    return get_overflow_width_length_pair_unit(scroll_x_step, unit_type);
}

void ContainerBox::draw_ui(){
    Rect2 rect = Rect2(Vector2(0, 0), Size2(calculate_total_width(), calculate_total_height()));
    draw_rect(rect, background_color);
}

double ContainerBox::get_width_length_pair_unit(LengthPair pair, Harmonia::Unit unit_type){
    if(unit_type == Harmonia::Unit::NOT_SET) return 0;

    if(parent == nullptr){
        return ContainerUnitConverter::get_width(pair, window_size.x, window_size, unit_type);
    }

    return ContainerUnitConverter::get_width(pair, parent->get_width(), window_size, unit_type);
}

double ContainerBox::get_height_length_pair_unit(LengthPair pair, Harmonia::Unit unit_type){
    if(unit_type == Harmonia::Unit::NOT_SET) return 0;

    if(parent == nullptr){
        return ContainerUnitConverter::get_height(pair, window_size.y, window_size, unit_type);
    }

    return ContainerUnitConverter::get_height(pair, parent->get_height(), window_size, unit_type);
}

AlertManager* ContainerBox::get_alert_manager(){
    return alert_manager;
}

void ContainerBox::set_alert_manager(AlertManager* manager){
    alert_manager = manager;
}

void ContainerBox::update_presentation(){
    if(parent == nullptr){
        parent = get_parent_container();
    }
    
    Vector2 new_size = Vector2(calculate_total_width(), calculate_total_height());
    ContainerBox::set_size(new_size);
    if(content_box){
        update_children_position(content_box->get_children());
        content_box->set_size(Vector2(get_width(), get_height())); // Set to 100%, 100% no padding.
        content_box->apply_overflowing();
        content_box->standalone = false;
        if(is_overflowed_x || is_overflowed_y){
            content_box->is_overflowed_x = is_overflowed_x;
            content_box->is_overflowed_y = is_overflowed_y;
            content_box->overflowing_size_x_px = get_overflow_x_size();
            content_box->overflowing_size_y_px = get_overflow_y_size();
            content_box->max_scroll_left_px = content_box->overflowing_size_x_px;
            content_box->max_scroll_top_px = content_box->overflowing_size_y_px;
        }else{
            content_box->is_overflowed_x = is_overflowed_x;
            content_box->is_overflowed_y = is_overflowed_y;
            content_box->overflowing_size_x_px = 0;
            content_box->overflowing_size_y_px = 0;
            content_box->max_scroll_left_px = 0;
            content_box->max_scroll_top_px = 0;
        }
    }else{
        update_children_position(get_children());
    }

    if(is_overflowed_x && horizontal_scroll){
        horizontal_scroll->set_visible(true);
    } else if(horizontal_scroll && !is_overflowed_x){
        horizontal_scroll->set_visible(false);
    }
    
    if(is_overflowed_y && vertical_scroll){
        vertical_scroll->set_visible(true);
    }else if(vertical_scroll && !is_overflowed_y){
        vertical_scroll->set_visible(false);
    }
    position_scrolls();
}

void ContainerBox::update_children_position(TypedArray<Node> children){
    Vector2 position = Vector2(0, get_padding_up()); // scrolling here if used...
    Vector2 overflow = Vector2(0, 0);
    Vector2 sum_child_sizes = Vector2(0, get_padding_up());

    if(content_box){
        position.x += content_box->scroll_left_px;
        position.y += content_box->scroll_top_px;
    }

    for (size_t i = 0; i < children.size(); i++)
    {
        auto current_child = children[i];
        if(auto* container = Object::cast_to<ContainerBox>(current_child)){
            double sum_y = container->get_height() + container->get_padding_up() + container->get_padding_down() + container->get_margin_up() + container->get_margin_down();
            double sum_x = container->get_width() + container->get_padding_left() + container->get_padding_right() + container->get_margin_left() + container->get_margin_right();

            if(container->position_type == Harmonia::Position::STATIC){
                position.y += container->get_margin_up();
                position.y += container->get_padding_up();
                container->set_position(Vector2(position.x + get_padding_left() + container->get_margin_left(),
                                                position.y));
                position.y += container->get_height() + container->get_margin_down() + container->get_padding_down();
                overflow.x = calculate_overflow(get_width(), sum_x, overflow.x);
                sum_child_sizes.y += sum_y;
            }else if(container->position_type == Harmonia::Position::ABSOLUTE){
                double pos_container_x = container->get_pos_x() + get_padding_left() + container->get_margin_left();
                double pos_container_y = container->get_pos_y() + get_padding_up() + container->get_margin_up();
                container->set_position(Vector2(pos_container_x, pos_container_y));
                // overflow check
                overflow.x = calculate_overflow(get_width(), pos_container_x+container->get_width()+container->get_margin_right()+container->get_padding_right(), overflow.x);
                overflow.y = calculate_overflow(get_height(), pos_container_y+container->get_height()+container->get_padding_down()+container->get_margin_down(), overflow.y);
            }else if(container-> position_type == Harmonia::Position::RELATIVE){
                position.y += container->get_margin_up();
                position.y += container->get_padding_up();
                double pos_container_x = position.x + get_padding_left() + container->get_margin_left() + container->get_pos_x();
                container->set_position(Vector2(pos_container_x,
                                                position.y + container->get_pos_y()));
                position.y += container->get_height() + container->get_margin_down() + container->get_padding_down();

                overflow.x = calculate_overflow(get_width(), sum_x+container->get_pos_x(), overflow.x);
                overflow.y = calculate_overflow(get_height(), sum_y+container->get_pos_y(), overflow.y);

                sum_child_sizes.y += sum_y;
            }
        }else if(auto* control = Object::cast_to<Control>(current_child)){
            double side_left = control->get_anchor(Side::SIDE_LEFT);
            double side_top = control->get_anchor(Side::SIDE_TOP);
            double side_right = control->get_anchor(Side::SIDE_RIGHT);
            double side_bottom = control->get_anchor(Side::SIDE_BOTTOM);

            if(side_left == 0 && side_top == 0 && side_right == 0 && side_bottom == 0){
                // Layout mode position
                control->set_position(Vector2(position.x + get_padding_left(), position.y));
            }else{
                control->set_position(Vector2(position.x, position.y));
                update_control_anchors(control);

                // Resets offsets to 0, this fixes the issue with content overflowing outside of the content area.
                // It disables the use of offsets, but might change in the future.
                control->set_offset(Side::SIDE_LEFT, 0);
                control->set_offset(Side::SIDE_TOP, 0);
                control->set_offset(Side::SIDE_RIGHT, 0);
                control->set_offset(Side::SIDE_BOTTOM, 0);
            }

            position.y += control->get_size().y;
        }
    }

    overflow.x = calculate_overflow(get_width(), sum_child_sizes.x, overflow.x);
    overflow.y = calculate_overflow(get_height(), sum_child_sizes.y, overflow.y);

    if(overflow.x > 0){
        is_overflowed_x = true;
        set_overflow_x_size(overflow.x);
    }else{
        is_overflowed_x = false;
        set_overflow_x_size(0);
    }

    if(overflow.y > 0){
        is_overflowed_y = true;
        set_overflow_y_size(overflow.y);
    }else{
        is_overflowed_y = false;
        set_overflow_y_size(0);
    }
}

void ContainerBox::update_control_anchors(Control* control){
    double side_left = control->get_anchor(Side::SIDE_LEFT);
    double side_top = control->get_anchor(Side::SIDE_TOP);
    double side_right = control->get_anchor(Side::SIDE_RIGHT);
    double side_bottom = control->get_anchor(Side::SIDE_BOTTOM);

    if(side_left == 0 && side_top == 0 && side_right == 0 && side_bottom == 0){
        return; // Layout mode is position
    }

    double left_min_size = get_padding_left()/calculate_total_width();
    double right_max_size = 1-get_padding_right()/calculate_total_width(); // 1 calculates to 100%
    double top_min_size = get_padding_up()/calculate_total_width();
    double bottom_max_size = 1-get_padding_down()/calculate_total_width();

    if(side_left < left_min_size){
        control->set_anchor(Side::SIDE_LEFT, left_min_size);
    }else if(side_left > right_max_size){
        control->set_anchor(Side::SIDE_LEFT, right_max_size);
    }

    if(side_right < left_min_size){
        control->set_anchor(Side::SIDE_RIGHT, left_min_size);
    }else if(side_right > right_max_size){
        control->set_anchor(Side::SIDE_RIGHT, right_max_size);
    }

    if(side_top < top_min_size){
        control->set_anchor(Side::SIDE_TOP, top_min_size);
    }else if(side_top > bottom_max_size){
        control->set_anchor(Side::SIDE_TOP, bottom_max_size);
    }   

    if(side_bottom < top_min_size){
        control->set_anchor(Side::SIDE_BOTTOM, top_min_size);
    }else if(side_bottom > bottom_max_size){
        control->set_anchor(Side::SIDE_BOTTOM, bottom_max_size);
    }
}

ContainerBox* ContainerBox::get_parent_container()
{
    Node* parent_node = get_parent();
    if(auto* container_box = Object::cast_to<ContainerBox>(parent_node))
    {
        return container_box;
    }

    // Basically retrieves parent of a content box if its inside of a content box. 
    Node* deep_parent = get_parent()->get_parent();
    if(auto* deep_container_box = Object::cast_to<ContainerBox>(deep_parent))
    {
        return deep_container_box;
    }

    return nullptr;
}

void ContainerBox::set_padding_str(String new_padding){
    List<String> paddings = split(new_padding, " ", true);
    int padding_size = paddings.size();
    padding_str = new_padding;
    if(padding_size == 1){
        LengthPair padding_all = LengthPair::get_pair(new_padding);
        set_padding_all(padding_all.length, padding_all.unit_type);
        if(debug_outputs) UtilityFunctions::print("Extracted 1 padding:", padding_all.length);
    }
    if(padding_size == 2){
        LengthPair padding_y = LengthPair::get_pair(paddings[0]);
        LengthPair padding_x = LengthPair::get_pair(paddings[1]);
        set_padding_y_vertical(padding_y.length, padding_y.unit_type, false); // false to avoid duplicate alert.
        set_padding_x_horizontal(padding_x.length, padding_x.unit_type);
        if(debug_outputs) UtilityFunctions::print("Extracted 2 paddings:", padding_y.length, padding_x.length);
    }
    if(padding_size == 3){
        padding_up = LengthPair::get_pair(paddings[0]);
        LengthPair padding_x = LengthPair::get_pair(paddings[1]);
        padding_down = LengthPair::get_pair(paddings[2]);
        set_padding_x_horizontal(padding_x.length, padding_x.unit_type);
        if(debug_outputs) UtilityFunctions::print("Extracted 3 paddings:", padding_up.length, padding_x.length, padding_down.length);
    }
    if(padding_size == 4){
        padding_up = LengthPair::get_pair(paddings[0]);
        padding_right = LengthPair::get_pair(paddings[1]);
        padding_down = LengthPair::get_pair(paddings[2]);
        padding_left = LengthPair::get_pair(paddings[3]);

        alert_manager->dispatch_alert(memnew(AlertLayoutChange(ALERT_LAYOUT_CHANGE, AlertLayoutChange::LayoutChanged::PADDING)));
        queue_redraw();
        if(debug_outputs) UtilityFunctions::print("Extracted 4 paddings:", padding_up.length, padding_right.length, padding_down.length, padding_left.length);
    }
    else{
        if(debug_outputs) UtilityFunctions::print("Wrong padding str, couldnt extract any paddings");
    }
}

String ContainerBox::get_padding_str(){
    return padding_str;
}

void ContainerBox::set_padding_all(double all_sides, Harmonia::Unit unit_type, bool dispatch_alert_and_queue){
    padding_up.length = all_sides;
    padding_up.unit_type = unit_type;
    padding_right.length = all_sides;
    padding_right.unit_type = unit_type;
    padding_down.length = all_sides;
    padding_down.unit_type = unit_type;
    padding_left.length = all_sides;
    padding_left.unit_type = unit_type;
    if (dispatch_alert_and_queue)
    {
        alert_manager->dispatch_alert(memnew(AlertLayoutChange(ALERT_LAYOUT_CHANGE, AlertLayoutChange::LayoutChanged::PADDING)));
        queue_redraw();
    }
}

void ContainerBox::set_padding_y_vertical(double vertical_y, Harmonia::Unit vertical_unit, bool dispatch_alert_and_queue){
    padding_up.length = vertical_y;
    padding_up.unit_type = vertical_unit;
    padding_down.length = vertical_y;
    padding_down.unit_type = vertical_unit;
    if (dispatch_alert_and_queue)
    {
        alert_manager->dispatch_alert(memnew(AlertLayoutChange(ALERT_LAYOUT_CHANGE, AlertLayoutChange::LayoutChanged::PADDING)));
        queue_redraw();
    }
}

void ContainerBox::set_padding_x_horizontal(double horizontal_x, Harmonia::Unit horizontal_unit, bool dispatch_alert_and_queue){
    padding_right.length = horizontal_x;
    padding_right.unit_type = horizontal_unit;
    padding_left.length = horizontal_x;
    padding_left.unit_type = horizontal_unit;
    if (dispatch_alert_and_queue)
    {
        alert_manager->dispatch_alert(memnew(AlertLayoutChange(ALERT_LAYOUT_CHANGE, AlertLayoutChange::LayoutChanged::PADDING)));
        queue_redraw();
    }
}

TypedArray<double> ContainerBox::get_paddings(Harmonia::Unit unit_type){
    // up right down left
    TypedArray<double> paddings;
    paddings[0] = get_padding_up(unit_type);
    paddings[1] = get_padding_right(unit_type);
    paddings[2] = get_padding_down(unit_type);
    paddings[3] = get_padding_left(unit_type);
    return paddings;
}

void ContainerBox::set_padding_up(double up, Harmonia::Unit up_unit, bool dispatch_alert_and_queue){
    padding_up.length = up;
    padding_up.unit_type = up_unit;
    if (dispatch_alert_and_queue)
    {
        alert_manager->dispatch_alert(memnew(AlertLayoutChange(ALERT_LAYOUT_CHANGE, AlertLayoutChange::LayoutChanged::PADDING)));
        queue_redraw();
    }
}

double ContainerBox::get_padding_up(Harmonia::Unit unit_type){
    return get_height_length_pair_unit(padding_up, unit_type);
}

void ContainerBox::set_padding_down(double down, Harmonia::Unit down_unit, bool dispatch_alert_and_queue){
    padding_down.length = down;
    padding_down.unit_type = down_unit;
    if (dispatch_alert_and_queue)
    {
        alert_manager->dispatch_alert(memnew(AlertLayoutChange(ALERT_LAYOUT_CHANGE, AlertLayoutChange::LayoutChanged::PADDING)));
        queue_redraw();
    }
}

double ContainerBox::get_padding_down(Harmonia::Unit unit_type){
    return get_height_length_pair_unit(padding_down, unit_type);
}

void ContainerBox::set_padding_left(double left, Harmonia::Unit left_unit, bool dispatch_alert_and_queue){
    padding_left.length = left;
    padding_left.unit_type = left_unit;
    if (dispatch_alert_and_queue)
    {
        alert_manager->dispatch_alert(memnew(AlertLayoutChange(ALERT_LAYOUT_CHANGE, AlertLayoutChange::LayoutChanged::PADDING)));
        queue_redraw();
    }
}

double ContainerBox::get_padding_left(Harmonia::Unit unit_type){
    return get_width_length_pair_unit(padding_left, unit_type);
}

void ContainerBox::set_padding_right(double right, Harmonia::Unit right_unit, bool dispatch_alert_and_queue){
    padding_right.length = right;
    padding_right.unit_type = right_unit;
    if (dispatch_alert_and_queue)
    {
        alert_manager->dispatch_alert(memnew(AlertLayoutChange(ALERT_LAYOUT_CHANGE, AlertLayoutChange::LayoutChanged::PADDING)));
        queue_redraw();
    }    
}

double ContainerBox::get_padding_right(Harmonia::Unit unit_type){
    return get_width_length_pair_unit(padding_right, unit_type);
}

void ContainerBox::set_margin_str(String new_margin){
    List<String> margins = split(new_margin, " ", true);
    int margin_size = margins.size();
    margin_str = new_margin;
    if(margin_size == 1){
        LengthPair margin_all = LengthPair::get_pair(new_margin);
        set_margin_all(margin_all.length, margin_all.unit_type);
        if(debug_outputs) UtilityFunctions::print("Extracted 1 margin:", margin_all.length);
    }
    if(margin_size == 2){
        LengthPair margin_y = LengthPair::get_pair(margins[0]);
        LengthPair margin_x = LengthPair::get_pair(margins[1]);
        set_margin_y_vertical(margin_y.length, margin_y.unit_type, false); // false to avoid duplicate alert.
        set_margin_x_horizontal(margin_x.length, margin_x.unit_type);
        if(debug_outputs) UtilityFunctions::print("Extracted 2 margins:", margin_y.length, margin_x.length);
    }
    if(margin_size == 3){
        margin_up = LengthPair::get_pair(margins[0]);
        LengthPair margin_x = LengthPair::get_pair(margins[1]);
        margin_down = LengthPair::get_pair(margins[2]);
        set_margin_x_horizontal(margin_x.length, margin_x.unit_type);
        if(debug_outputs) UtilityFunctions::print("Extracted 3 margins:", margin_up.length, margin_x.length, margin_down.length);
    }
    if(margin_size == 4){
        margin_up = LengthPair::get_pair(margins[0]);
        margin_right = LengthPair::get_pair(margins[1]);
        margin_down = LengthPair::get_pair(margins[2]);
        margin_left = LengthPair::get_pair(margins[3]);
        if(debug_outputs) UtilityFunctions::print("Extracted 4 margins:", margin_up.length, margin_right.length, margin_down.length, margin_left.length);
    }
    else{
        if(debug_outputs) UtilityFunctions::print("Wrong margin str, couldnt extract any margins");
    }
}

String ContainerBox::get_margin_str(){
    return margin_str;
}

void ContainerBox::set_margin_all(double all_sides, Harmonia::Unit unit_type, bool dispatch_alert){
    margin_up.length = all_sides;
    margin_up.unit_type = unit_type;
    margin_right.length = all_sides;
    margin_right.unit_type = unit_type;
    margin_down.length = all_sides;
    margin_down.unit_type = unit_type;
    margin_left.length = all_sides;
    margin_left.unit_type = unit_type;
    if (dispatch_alert) alert_manager->dispatch_alert(memnew(AlertLayoutChange(ALERT_LAYOUT_CHANGE, AlertLayoutChange::LayoutChanged::MARGIN)));
}

void ContainerBox::set_margin_y_vertical(double vertical_y, Harmonia::Unit vertical_unit, bool dispatch_alert){
    margin_up.length = vertical_y;
    margin_up.unit_type = vertical_unit;
    margin_down.length = vertical_y;
    margin_down.unit_type = vertical_unit;
    if (dispatch_alert) alert_manager->dispatch_alert(memnew(AlertLayoutChange(ALERT_LAYOUT_CHANGE, AlertLayoutChange::LayoutChanged::MARGIN)));
}

void ContainerBox::set_margin_x_horizontal(double horizontal_x, Harmonia::Unit horizontal_unit, bool dispatch_alert){
    margin_right.length = horizontal_x;
    margin_right.unit_type = horizontal_unit;
    margin_left.length = horizontal_x;
    margin_left.unit_type = horizontal_unit;
    if (dispatch_alert) alert_manager->dispatch_alert(memnew(AlertLayoutChange(ALERT_LAYOUT_CHANGE, AlertLayoutChange::LayoutChanged::MARGIN)));
}

TypedArray<double> ContainerBox::get_margins(Harmonia::Unit unit_type){
    // up right down left
    TypedArray<double> margins;
    margins[0] = get_margin_up(unit_type);
    margins[1] = get_margin_right(unit_type);
    margins[2] = get_margin_down(unit_type);
    margins[3] = get_margin_left(unit_type);
    return margins;
}

void ContainerBox::set_margin_up(double up, Harmonia::Unit up_unit, bool dispatch_alert){
    margin_up.length = up;
    margin_up.unit_type = up_unit;
    if (dispatch_alert) alert_manager->dispatch_alert(memnew(AlertLayoutChange(ALERT_LAYOUT_CHANGE, AlertLayoutChange::LayoutChanged::MARGIN)));
}

double ContainerBox::get_margin_up(Harmonia::Unit unit_type){
    return get_height_length_pair_unit(margin_up, unit_type);
}

void ContainerBox::set_margin_down(double down, Harmonia::Unit down_unit, bool dispatch_alert){
    margin_down.length = down;
    margin_down.unit_type = down_unit;
    if (dispatch_alert) alert_manager->dispatch_alert(memnew(AlertLayoutChange(ALERT_LAYOUT_CHANGE, AlertLayoutChange::LayoutChanged::MARGIN)));
}

double ContainerBox::get_margin_down(Harmonia::Unit unit_type){
    return get_height_length_pair_unit(margin_down, unit_type);
}

void ContainerBox::set_margin_left(double left, Harmonia::Unit left_unit, bool dispatch_alert){
    margin_left.length = left;
    margin_left.unit_type = left_unit;
    if (dispatch_alert) alert_manager->dispatch_alert(memnew(AlertLayoutChange(ALERT_LAYOUT_CHANGE, AlertLayoutChange::LayoutChanged::MARGIN)));
}
double ContainerBox::get_margin_left(Harmonia::Unit unit_type){
    return get_width_length_pair_unit(margin_left, unit_type);
}

void ContainerBox::set_margin_right(double right, Harmonia::Unit right_unit, bool dispatch_alert){
    margin_right.length = right;
    margin_right.unit_type = right_unit;
    if (dispatch_alert) alert_manager->dispatch_alert(memnew(AlertLayoutChange(ALERT_LAYOUT_CHANGE, AlertLayoutChange::LayoutChanged::MARGIN)));    
}

double ContainerBox::get_margin_right(Harmonia::Unit unit_type){
    return get_width_length_pair_unit(margin_right, unit_type);
}

Color ContainerBox::get_background_color(){
    return background_color;
}

void ContainerBox::set_background_color(Color color){
    background_color = color;
    queue_redraw();
}

void ContainerBox::set_position_type(Harmonia::Position new_type){
    position_type = new_type;
}
Harmonia::Position ContainerBox::get_position_type(){
    return position_type;
}

void ContainerBox::set_overflow_behaviour(Harmonia::OverflowBehaviour behaviour){
    overflow_behaviour = behaviour;
}

Harmonia::OverflowBehaviour ContainerBox::get_overflow_behaviour(){
    return overflow_behaviour;
}

void ContainerBox::set_pos_x(double new_x, Harmonia::Unit unit_type){
    pos_x.length = new_x;
    pos_x.unit_type = unit_type;
    alert_manager->dispatch_alert(memnew(AlertLayoutChange(ALERT_LAYOUT_CHANGE, AlertLayoutChange::LayoutChanged::POSITION)));
}

double ContainerBox::get_pos_x(Harmonia::Unit unit_type){
    return get_width_length_pair_unit(pos_x, unit_type);
}

void ContainerBox::set_pos_x_str(String new_x){
    pos_x_str = new_x;
    LengthPair pos_x = LengthPair::get_pair(new_x);
    set_pos_x(pos_x.length, pos_x.unit_type);
}

String ContainerBox::get_pos_x_str(){
    return pos_x_str;
}

void ContainerBox::set_pos_y(double new_y, Harmonia::Unit unit_type){
    pos_y.length = new_y;
    pos_y.unit_type = unit_type;
    alert_manager->dispatch_alert(memnew(AlertLayoutChange(ALERT_LAYOUT_CHANGE, AlertLayoutChange::LayoutChanged::POSITION)));
}

double ContainerBox::get_pos_y(Harmonia::Unit unit_type){
    return get_height_length_pair_unit(pos_y, unit_type);
}

void ContainerBox::set_pos_y_str(String new_y){
    pos_y_str = new_y;
    LengthPair pos_y = LengthPair::get_pair(new_y);
    set_pos_y(pos_y.length, pos_y.unit_type);
}

String ContainerBox::get_pos_y_str(){
    return pos_y_str;
}

void ContainerBox::set_debug_outputs(bool debug_outputs)
{
    ContainerBox::debug_outputs = debug_outputs;
}

bool ContainerBox::get_debug_outputs()
{
    return ContainerBox::debug_outputs;
}

double ContainerBox::get_width(Harmonia::Unit unit_type){
    return get_width_length_pair_unit(width, unit_type);
}

double ContainerBox::calculate_total_width(Harmonia::Unit unit_type){
    return get_width_length_pair_unit(width, unit_type) + get_padding_left(unit_type) + get_padding_right(unit_type);
}

void ContainerBox::set_width(double length, Harmonia::Unit unit_type){
    ContainerBox::width.length = length;
    ContainerBox::width.unit_type = unit_type;
    alert_manager->dispatch_alert(memnew(AlertLayoutChange(ALERT_LAYOUT_CHANGE, AlertLayoutChange::LayoutChanged::WIDTH)));
}

void ContainerBox::set_width_str(String length_and_unit){
    width_str = length_and_unit;
    LengthPair pair = LengthPair::get_pair(length_and_unit);
    set_width(pair.length, pair.unit_type);
}

String ContainerBox::get_width_str()
{
    return width_str;
}

void ContainerBox::set_height(double length, Harmonia::Unit unit_type){
    ContainerBox::height.length = length;
    ContainerBox::height.unit_type = unit_type;
    alert_manager->dispatch_alert(memnew(AlertLayoutChange(ALERT_LAYOUT_CHANGE, AlertLayoutChange::LayoutChanged::HEIGHT)));
}

void ContainerBox::set_height_str(String length_and_unit){
    height_str = length_and_unit; 
    LengthPair pair = LengthPair::get_pair(length_and_unit);
    set_height(pair.length, pair.unit_type);
}

String ContainerBox::get_height_str()
{
    return height_str;
}

double ContainerBox::get_height(Harmonia::Unit unit_type){
    return get_height_length_pair_unit(height, unit_type);
}

double ContainerBox::calculate_total_height(Harmonia::Unit unit_type){
    return get_height_length_pair_unit(height, unit_type) + get_padding_up(unit_type) + get_padding_down(unit_type);
}

void ContainerBox::_notification(int p_what)
{
    if(p_what == NOTIFICATION_READY){
        _ready();
    }else if(p_what == NOTIFICATION_PROCESS){
        _process(get_process_delta_time());
    }else if(p_what == NOTIFICATION_RESIZED){
        // Process to convert new size to current unit size.
    }else if(p_what == NOTIFICATION_DRAW){
        draw_ui();
    }
}

void ContainerBox::_bind_methods(){
    ClassDB::bind_method(D_METHOD("on_window_size_changed"), &ContainerBox::on_window_size_changed);

    ClassDB::bind_method(D_METHOD("set_width", "length", "unit_type"), &ContainerBox::set_width);
    ClassDB::bind_method(D_METHOD("get_width", "unit_type"), &ContainerBox::get_width);
    ClassDB::bind_method(D_METHOD("set_height", "length", "unit_type"), &ContainerBox::set_height);
    ClassDB::bind_method(D_METHOD("get_height", "unit_type"), &ContainerBox::get_height);

    ClassDB::bind_method(D_METHOD("set_width_str", "length_and_unit"), &ContainerBox::set_width_str);
    ClassDB::bind_method(D_METHOD("get_width_str"), &ContainerBox::get_width_str);    
    ClassDB::bind_method(D_METHOD("set_height_str", "length_and_unit"), &ContainerBox::set_height_str);
    ClassDB::bind_method(D_METHOD("get_height_str"), &ContainerBox::get_height_str);

    ClassDB::bind_method(D_METHOD("set_debug_outputs", "debug_outputs"), &ContainerBox::set_debug_outputs);
    ClassDB::bind_method(D_METHOD("get_debug_outputs"), &ContainerBox::get_debug_outputs);

    ClassDB::bind_method(D_METHOD("set_alert_manager", "manager"), &ContainerBox::set_alert_manager);
    ClassDB::bind_method(D_METHOD("get_alert_manager"), &ContainerBox::get_alert_manager);

    ClassDB::bind_method(D_METHOD("set_position_type", "new_type"), &ContainerBox::set_position_type);
    ClassDB::bind_method(D_METHOD("get_position_type"), &ContainerBox::get_position_type);

    ClassDB::bind_method(D_METHOD("set_pos_x_str", "new_x"), &ContainerBox::set_pos_x_str);
    ClassDB::bind_method(D_METHOD("get_pos_x_str"), &ContainerBox::get_pos_x_str);
    ClassDB::bind_method(D_METHOD("set_pos_y_str", "new_y"), &ContainerBox::set_pos_y_str);
    ClassDB::bind_method(D_METHOD("get_pos_y_str"), &ContainerBox::get_pos_y_str);
    
    ClassDB::bind_method(D_METHOD("set_pos_x", "new_x", "unit_type"), &ContainerBox::set_pos_x);
    ClassDB::bind_method(D_METHOD("get_pos_x", "unit_type"), &ContainerBox::get_pos_x);
    ClassDB::bind_method(D_METHOD("set_pos_y", "new_y", "unit_type"), &ContainerBox::set_pos_y);
    ClassDB::bind_method(D_METHOD("get_pos_y", "unit_type"), &ContainerBox::get_pos_y);

    ClassDB::bind_method(D_METHOD("set_overflow_behaviour", "behaviour"), &ContainerBox::set_overflow_behaviour);
    ClassDB::bind_method(D_METHOD("get_overflow_behaviour"), &ContainerBox::get_overflow_behaviour);
    
    ClassDB::bind_method(D_METHOD("set_horizontal_scroll", "scroll"), &ContainerBox::set_horizontal_scroll);
    ClassDB::bind_method(D_METHOD("get_horizontal_scroll"), &ContainerBox::get_horizontal_scroll);
    ClassDB::bind_method(D_METHOD("set_vertical_scroll", "scroll"), &ContainerBox::set_vertical_scroll);
    ClassDB::bind_method(D_METHOD("get_vertical_scroll"), &ContainerBox::get_vertical_scroll);
    ClassDB::bind_method(D_METHOD("set_string_scroll_y_step", "scroll_y_step"), &ContainerBox::set_string_scroll_y_step);
    ClassDB::bind_method(D_METHOD("get_string_scroll_y_step"), &ContainerBox::get_string_scroll_y_step);
    ClassDB::bind_method(D_METHOD("set_string_scroll_x_step", "scroll_x_step"), &ContainerBox::set_string_scroll_x_step);
    ClassDB::bind_method(D_METHOD("get_string_scroll_x_step"), &ContainerBox::get_string_scroll_x_step);

    ClassDB::bind_method(D_METHOD("set_margin_all", "all_sides", "unit_type", "dispatch_alert"), &ContainerBox::set_margin_all, DEFVAL(true));
    ClassDB::bind_method(D_METHOD("set_margin_y_vertical", "vertical_y", "vertical_unit", "dispatch_alert"), &ContainerBox::set_margin_y_vertical, DEFVAL(true));
    ClassDB::bind_method(D_METHOD("set_margin_x_horizontal", "horizontal_x", "horizontal_unit", "dispatch_alert"), &ContainerBox::set_margin_x_horizontal, DEFVAL(true));
    ClassDB::bind_method(D_METHOD("get_margins", "unit_type"), &ContainerBox::get_margins, DEFVAL(Harmonia::PIXEL));

    ClassDB::bind_method(D_METHOD("set_margin_str", "new_margin"), &ContainerBox::set_margin_str);
    ClassDB::bind_method(D_METHOD("get_margin_str"), &ContainerBox::get_margin_str);
    ClassDB::bind_method(D_METHOD("set_margin_up", "up_margin", "up_unit", "dispatch_alert"), &ContainerBox::set_margin_up, DEFVAL(true));
    ClassDB::bind_method(D_METHOD("get_margin_up", "unit_type"), &ContainerBox::get_margin_up, DEFVAL(Harmonia::PIXEL));
    ClassDB::bind_method(D_METHOD("set_margin_right", "right_margin", "right_unit", "dispatch_alert"), &ContainerBox::set_margin_right, DEFVAL(true));
    ClassDB::bind_method(D_METHOD("get_margin_right", "unit_type"), &ContainerBox::get_margin_right, DEFVAL(Harmonia::PIXEL));
    ClassDB::bind_method(D_METHOD("set_margin_down", "down_margin", "down_unit", "dispatch_alert"), &ContainerBox::set_margin_down, DEFVAL(true));
    ClassDB::bind_method(D_METHOD("get_margin_down", "unit_type"), &ContainerBox::get_margin_down, DEFVAL(Harmonia::PIXEL));
    ClassDB::bind_method(D_METHOD("set_margin_left", "left_margin", "left_unit", "dispatch_alert"), &ContainerBox::set_margin_left, DEFVAL(true));
    ClassDB::bind_method(D_METHOD("get_margin_left", "unit_type"), &ContainerBox::get_margin_left, DEFVAL(Harmonia::PIXEL));
    
    ClassDB::bind_method(D_METHOD("set_padding_all", "all_sides", "unit_type", "dispatch_alert"), &ContainerBox::set_padding_all, DEFVAL(true));
    ClassDB::bind_method(D_METHOD("set_padding_y_vertical", "vertical_y", "vertical_unit", "dispatch_alert"), &ContainerBox::set_padding_y_vertical, DEFVAL(true));
    ClassDB::bind_method(D_METHOD("set_padding_x_horizontal", "horizontal_x", "horizontal_unit", "dispatch_alert"), &ContainerBox::set_padding_x_horizontal, DEFVAL(true));
    ClassDB::bind_method(D_METHOD("get_paddings", "unit_type"), &ContainerBox::get_paddings, DEFVAL(Harmonia::PIXEL));

    ClassDB::bind_method(D_METHOD("set_padding_str", "new_padding"), &ContainerBox::set_padding_str);
    ClassDB::bind_method(D_METHOD("get_padding_str"), &ContainerBox::get_padding_str);
    ClassDB::bind_method(D_METHOD("set_padding_up", "up_padding", "up_unit", "dispatch_alert"), &ContainerBox::set_padding_up, DEFVAL(true));
    ClassDB::bind_method(D_METHOD("get_padding_up", "unit_type"), &ContainerBox::get_padding_up, DEFVAL(Harmonia::PIXEL));
    ClassDB::bind_method(D_METHOD("set_padding_right", "right_padding", "right_unit", "dispatch_alert"), &ContainerBox::set_padding_right, DEFVAL(true));
    ClassDB::bind_method(D_METHOD("get_padding_right", "unit_type"), &ContainerBox::get_padding_right, DEFVAL(Harmonia::PIXEL));
    ClassDB::bind_method(D_METHOD("set_padding_down", "down_padding", "down_unit", "dispatch_alert"), &ContainerBox::set_padding_down, DEFVAL(true));
    ClassDB::bind_method(D_METHOD("get_padding_down", "unit_type"), &ContainerBox::get_padding_down, DEFVAL(Harmonia::PIXEL));
    ClassDB::bind_method(D_METHOD("set_padding_left", "left_padding", "left_unit", "dispatch_alert"), &ContainerBox::set_padding_left, DEFVAL(true));
    ClassDB::bind_method(D_METHOD("get_padding_left", "unit_type"), &ContainerBox::get_padding_left, DEFVAL(Harmonia::PIXEL));

    ClassDB::bind_method(D_METHOD("set_background_color", "color"), &ContainerBox::set_background_color);
    ClassDB::bind_method(D_METHOD("get_background_color"), &ContainerBox::get_background_color);

    ClassDB::bind_method(D_METHOD("update_presentation"), &ContainerBox::update_presentation);

    const String positioning_types = "STATIC:0,ABSOLUTE:1,RELATIVE:2";
    const String overflow_behaviours = "SCROLL:0,HIDDEN:1,VISIBLE:2";

    ADD_PROPERTY(PropertyInfo(Variant::OBJECT, "alert_manager", PROPERTY_HINT_RESOURCE_TYPE, "alert_manager", PROPERTY_USAGE_NO_EDITOR), "set_alert_manager", "get_alert_manager");
    ADD_PROPERTY(PropertyInfo(Variant::STRING, "width_str", PROPERTY_HINT_TYPE_STRING, "width_str", PROPERTY_USAGE_NO_EDITOR), "set_width_str", "get_width_str");
    ADD_PROPERTY(PropertyInfo(Variant::STRING, "height_str", PROPERTY_HINT_TYPE_STRING, "height_str", PROPERTY_USAGE_NO_EDITOR), "set_height_str", "get_height_str");
    ADD_PROPERTY(PropertyInfo(Variant::BOOL, "debug_outputs", PROPERTY_HINT_TYPE_STRING, "debug_outputs", PROPERTY_USAGE_NO_EDITOR), "set_debug_outputs", "get_debug_outputs");
    ADD_PROPERTY(PropertyInfo(Variant::INT, "overflow_behaviour", PROPERTY_HINT_ENUM, overflow_behaviours, PROPERTY_USAGE_DEFAULT), "set_overflow_behaviour", "get_overflow_behaviour");
    ADD_PROPERTY(PropertyInfo(Variant::STRING, "string_scroll_y_step", PROPERTY_HINT_TYPE_STRING, "string_scroll_y_step", PROPERTY_USAGE_NO_EDITOR), "set_string_scroll_y_step", "get_string_scroll_y_step");
    ADD_PROPERTY(PropertyInfo(Variant::STRING, "string_scroll_x_step", PROPERTY_HINT_TYPE_STRING, "string_scroll_x_step", PROPERTY_USAGE_NO_EDITOR), "set_string_scroll_x_step", "get_string_scroll_x_step");
    ADD_PROPERTY(PropertyInfo(Variant::NODE_PATH, "vertical_scroll", PROPERTY_HINT_NODE_TYPE, "vertical_scroll", PROPERTY_USAGE_DEFAULT), "set_vertical_scroll", "get_vertical_scroll");
    ADD_PROPERTY(PropertyInfo(Variant::NODE_PATH, "horizontal_scroll", PROPERTY_HINT_NODE_TYPE, "horizontal_scroll", PROPERTY_USAGE_DEFAULT), "set_horizontal_scroll", "get_horizontal_scroll");    
    ADD_PROPERTY(PropertyInfo(Variant::INT, "positioning", PROPERTY_HINT_ENUM, positioning_types, PROPERTY_USAGE_DEFAULT), "set_position_type", "get_position_type");
    ADD_PROPERTY(PropertyInfo(Variant::STRING, "pos_x_str", PROPERTY_HINT_TYPE_STRING, "pos_x_str", PROPERTY_USAGE_NO_EDITOR), "set_pos_x_str", "get_pos_x_str");
    ADD_PROPERTY(PropertyInfo(Variant::STRING, "pos_y_str", PROPERTY_HINT_TYPE_STRING, "pos_y_str", PROPERTY_USAGE_NO_EDITOR), "set_pos_y_str", "get_pos_y_str");
    ADD_PROPERTY(PropertyInfo(Variant::STRING, "margin_str", PROPERTY_HINT_TYPE_STRING, "margin_str", PROPERTY_USAGE_NO_EDITOR), "set_margin_str", "get_margin_str");
    ADD_PROPERTY(PropertyInfo(Variant::STRING, "padding_str", PROPERTY_HINT_TYPE_STRING, "padding_str", PROPERTY_USAGE_NO_EDITOR), "set_padding_str", "get_padding_str");
    ADD_PROPERTY(PropertyInfo(Variant::COLOR, "background_color", PROPERTY_HINT_NONE, "background_color", PROPERTY_USAGE_NO_EDITOR), "set_background_color", "get_background_color");
}

bool ContainerBox::_set(const StringName &p_name, const Variant &p_value)
{
    String name = p_name;
	if (name == "width_str") {
		set_width_str(p_value);
		return true;
	}else if(name == "height_str"){
        set_height_str(p_value);
        return true;
    }else if(name == "margin_str"){
        set_margin_str(p_value);
        return true;
    }else if(name == "padding_str"){
        set_padding_str(p_value);
        return true;
    }else if(name == "background_color"){
        set_background_color(p_value);
        return true;
    }else if(name == "debug_outputs"){
        set_debug_outputs(p_value);
    	return true;
    }else if(name == "pos_x_str"){
        set_pos_x_str(p_value);
        return true;
    }else if(name == "pos_y_str"){
        set_pos_y_str(p_value);
        return true;
    }else if(name=="string_scroll_x_step"){
        set_string_scroll_x_step(p_value);
        return true;
    }else if(name=="string_scroll_y_step"){
        set_string_scroll_y_step(p_value);
        return true;
    }
	return false;
}

bool ContainerBox::_get(const StringName &p_name, Variant &r_ret) const
{
    String name = p_name;
	if (name == "width_str") {
	    r_ret = width_str;
		return true;
	}else if(name == "height_str"){
        r_ret = height_str;
        return true;
    }else if(name == "margin_str"){
        r_ret = margin_str;
        return true;
    }else if(name == "padding_str"){
        r_ret = padding_str;
        return true;
    }else if(name == "background_color"){
        r_ret = background_color;
        return true;
    }else if(name == "debug_outputs"){
        r_ret = debug_outputs;
    	return true;
    }else if(name == "pos_x_str"){
        r_ret = pos_x_str;
        return true;
    }else if(name == "pos_y_str"){
        r_ret = pos_y_str;
        return true;
    }else if(name=="string_scroll_x_step"){
        r_ret = string_scroll_x_step;
        return true;
    }else if(name=="string_scroll_y_step"){
        r_ret = string_scroll_y_step;
        return true;
    }
	return false;
}

void ContainerBox::_get_property_list(List<PropertyInfo> *p_list) const
{
    p_list->push_back(PropertyInfo(Variant::STRING, "string_scroll_x_step"));
    p_list->push_back(PropertyInfo(Variant::STRING, "string_scroll_y_step"));
    p_list->push_back(PropertyInfo(Variant::STRING, "width_str"));
    p_list->push_back(PropertyInfo(Variant::STRING, "height_str"));
    p_list->push_back(PropertyInfo(Variant::STRING, "margin_str"));
    p_list->push_back(PropertyInfo(Variant::STRING, "padding_str"));
    p_list->push_back(PropertyInfo(Variant::COLOR, "background_color"));
    p_list->push_back(PropertyInfo(Variant::BOOL, "debug_outputs"));
    p_list->push_back(PropertyInfo(Variant::STRING, "pos_x_str"));
    p_list->push_back(PropertyInfo(Variant::STRING, "pos_y_str"));
}
