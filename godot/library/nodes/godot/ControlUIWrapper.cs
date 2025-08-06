using Godot;

namespace HarmoniaUI.library.nodes.godot
{
    /// <summary>
    /// Simple wrapper around a <see cref="_primitive"/> control node
    /// </summary>
    /// <remarks>
    /// Should have a completely flat/empty style so it behaves like regular node
    /// </remarks>
    public partial class ControlUIWrapper : UINode
    {
        public ControlUIWrapper() { }
        public ControlUIWrapper(Control primitive)
        {
            _primitive = primitive;
            ParsedStyle = new(); // Does this work?
        }

        private readonly Control _primitive;

        /// TODO? Conditional compilation to remove this from release?
        // Checks nulls to prevent editor errors. \/

        public override float ContentWidth
        {
            get
            {
                if (_primitive == null || !GodotObject.IsInstanceValid(_primitive)) return 0;
                return _primitive.Size.X;
            } 
            set
            {
                if(_primitive == null || !GodotObject.IsInstanceValid(_primitive)) return;
                _primitive.Size = new(value, _primitive.Size.Y);
            }
        }
        public override float ContentHeight
        {
            get
            {
                if(_primitive == null || !GodotObject.IsInstanceValid(_primitive)) return 0;
                return _primitive.Size.Y;
            }
            set
            {
                if(_primitive == null || !GodotObject.IsInstanceValid(_primitive)) return;
                _primitive.Size = new(_primitive.Size.X, value);
            }
        }
    }
}
