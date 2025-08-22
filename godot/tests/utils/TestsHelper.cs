using GdUnit4;
using GdUnit4.Core.Execution.Exceptions;
using System.Threading.Tasks;

namespace HarmoniaUI.Tests.Utils
{
    /// <summary>
    /// Helps with running tests and processing test nodes.
    /// </summary>
    internal class TestsHelper
    {
        /// <summary>
        /// Processes <paramref name="tests"/>, waits for a draw signal, redraws and waits for all tests to finish.
        /// Throws an exception if any test fails, otherwise does nothing and passes the test.
        /// </summary>
        /// <param name="tests">TestRepo to be processed</param>
        /// <returns>Test task</returns>
        /// <exception cref="TestFailedException">Fails if no tests, or if any of the tests fail</exception>
        public static async Task ProcessTestRepo(TestsRepo tests)
        {
            if (tests == null)
            {
                throw new TestFailedException("Root node is not a tests repo");
            }

            tests.RestartAll();
            tests.QueueRedraw();
            await tests.AwaitSignal("draw");

            if (tests.TestNodes == null) throw new TestFailedException("No test nodes found, add it or remove the test entirely.");
            while (true)
            {
                if (!tests.AllTested)
                {
                    continue;
                }

                if (tests.AllPassed) break;
                else
                {
                    throw new TestFailedException(tests.FailedMessage);
                }                
            }
        }
    }
}
