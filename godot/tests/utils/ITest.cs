using System;

namespace HarmoniaUI.Tests.Utils
{
    /// <summary>
    /// Defines a contract for a testing class.
    /// <para>
    /// Includes information neccesary for passing/failing a test and some other debug information.
    /// </para>
    /// </summary>
    public interface ITest
    {
        /// <summary>
        /// Represents the method that will handle the OnTestFinished event.
        /// </summary>
        /// <param name="passed">True if the test passed; otherwise false.</param>
        public delegate void TestFinishedEventHandler(bool passed);

        /// <summary>
        /// Occurs when the test has finished.
        /// </summary>
        public event TestFinishedEventHandler OnTestFinished;

        /// <summary>
        /// A name of the tests, might be displayed in the fail errors.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Whether the test passed or failed,
        /// True if passed, False if failed
        /// </summary>
        public bool Passed { get; set; }

        /// <summary>
        /// Whether the test ran already once or more.
        /// </summary>
        public bool Tested { get; set; }

        /// <summary>
        /// Restarts the test, allowing for a clean rerun under new conditions.
        /// </summary>
        public void Restart();
    }
}