using Godot;
using HarmoniaUI.Tests.Utils;
using System;

/// <summary>
/// Repository for nodes with <see cref="ITest"/>, store multiple tests in a single scene
/// </summary>
/// <remarks>
/// That simplifies test processing, since the testing class doesn't need to find all <see cref="ITest"/> nodes.
/// </remarks>
public partial class TestsRepo : Control
{
    /// <summary>
    /// Nodes that are <see cref="ITest"/>
    /// </summary>
    [Export] public HarmoniaTest[] TestNodes { get; set; }

    /// <summary>
    /// Describes whether all <see cref="Tests"/> were positive (Passed).
    /// </summary>
    public bool AllPassed { get; set; }

    /// <summary>
    /// Describes whether all <see cref="Tests"/> were tested at least once
    /// </summary>
    public bool AllTested { get; set; }

    /// <summary>
    /// Message of failure composed from the <see cref="Tests"/>, allows for a more detailed
    /// Test failure. (More detailed compared to the regular failure)
    /// </summary>
    public string FailedMessage { get; set; }
    
    /// <summary>
    /// Converts the tests and prepares for it.
    /// </summary>
    public override void _Ready()
    {
        ConvertTests();
    }

    /// <summary>
    /// Converts all <see cref="TestNodes"/> to <see cref="ITest"/> and stores them in <see cref="Tests"/>
    /// </summary>
    /// <remarks>
    /// Throws an exception whenether there are no tests
    /// </remarks>
    /// <exception cref="Exception">When there are no test nodes</exception>
    private void ConvertTests()
    {
        if(TestNodes.Length == 0)
        {
            GD.PrintErr("TestsRepo - no tests found, put test nodes in the editor.");
            throw new Exception("TestsRepo - no tests found, put test nodes in the editor.");
        }

        for (int i = 0; i < TestNodes.Length; i++)
        {
            var testNode = TestNodes[i];
            testNode.OnTestFinished += Test_OnTestFinished;
        }
    }

    /// <summary>
    /// Checks for the test success, if all tests were tested, it marks
    /// <see cref="AllTested"/>, and if all tests passed it sets <see cref="AllPassed"/>
    /// <para>
    /// If any test fails it appends details of the test to <see cref="FailedMessage"/>
    /// This can be used to fail tests with a detailed message.
    /// </para>
    /// </summary>
    /// <param name="result">Result of the test, unused</param>
    private void Test_OnTestFinished(bool result)
    {
        string failMessage = "";
        foreach (var test in TestNodes)
        {
            if (!test.Tested) return;
            if (!test.Passed)
            {
                failMessage += $"\nTest {test.Name} failed!";
            }
        }

        if(failMessage != "")
        {
            AllPassed = false;
            FailedMessage = failMessage;
        }
        else
        {
            AllPassed = true;
        }
        AllTested = true;
    }
}
