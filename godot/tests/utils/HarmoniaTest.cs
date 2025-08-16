using Godot;
using System;
using static HarmoniaUI.Tests.Utils.ITest;

namespace HarmoniaUI.Tests.Utils
{
    /// <summary>
    /// Base node for any harmonia tests that are being performed in the editor using nodes.
    /// <para>
    /// <h2>Creating a custom test</h2>
    /// This class is very open for new tests. To create a custom tests, simply inherit from it.
    /// Call <see cref="PassTest"/> or <see cref="FailTest"/> to pass or fail tests, These will invoke
    /// <see cref="OnTestFinished"/> which other classes can listen to, those methods are virtual 
    /// as well (you can override them).
    /// </para>
    /// <para>
    /// For restarting a test, just call <see cref="Restart"/>, if you need to include your own logic, you can
    /// do that by overriding the restart method.
    /// </para>
    /// </summary>
    /// <remarks>
    /// This class directly doesn't introduce any tests, and shouldn't really be used.
    /// </remarks>
    [GlobalClass]
    public partial class HarmoniaTest : Control, ITest
    {
        public event TestFinishedEventHandler OnTestFinished;
        string ITest.Name { get => Name; set => Name = value; }
        public bool Passed { get; set; }
        public bool Tested { get; set; }

        public virtual void Restart()
        {
            Passed = false; 
            Tested = false;
        }

        /// <summary>
        /// Passes the test and invokes <see cref="OnTestFinished"/> with the result being true, because it passed
        /// </summary>
        public virtual void PassTest()
        {
            Tested = true;
            Passed = true;
            OnTestFinished?.Invoke(true);
        }

        /// <summary>
        /// Fails the test and invokes <see cref="OnTestFinished"/> with the result being false, because it failed
        /// </summary>
        public virtual void FailTest()
        {
            Tested = true;
            Passed = false;
            OnTestFinished?.Invoke(false);
        }
    }
}
