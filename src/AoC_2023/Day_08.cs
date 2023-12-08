using System.Text.RegularExpressions;

namespace AoC_2023;

public partial class Day_08 : BaseDay
{
    [GeneratedRegex(@"(?<Node>\w*(?=\s=)).*(?<Left>(?<=\()\w*(?=,)).*(?<Right>(?<=\s)\w*(?=\)))\)")]
    private static partial Regex NodeRegex();

    private readonly Regex _nodeRegex = NodeRegex();

    private sealed record BinaryTreeNode(string Id)
    {
        public BinaryTreeNode Left { get; set; } = null!;

        public BinaryTreeNode Right { get; set; } = null!;
    }

    private readonly (string Instructions, BinaryTreeNode Root) _input;

    const string Start = "AAA";
    const string End = "ZZZ";

    public Day_08()
    {
        _input = ParseInput();
    }

    public override ValueTask<string> Solve_1() => new($"{Solve_1_Original()}");

    public override ValueTask<string> Solve_2() => new($"{Solve_2_Original()}");

    public int Solve_1_Original()
    {
        int counter = 0;
        BinaryTreeNode currentNode = _input.Root;

        while (true)
        {
            currentNode = _input.Instructions[counter++ % _input.Instructions.Length] switch
            {
                'L' => currentNode.Left,
                'R' => currentNode.Right,
                _ => throw new SolvingException()
            };

            if (currentNode.Id == End)
            {
                return counter;
            }
        }
    }

    public int Solve_2_Original()
    {
        int result = 0;

        return result;
    }

    private (string, BinaryTreeNode) ParseInput()
    {
        var allGroupsOfLines = ParsedFile.ReadAllGroupsOfLines(InputFilePath);
        if (allGroupsOfLines[0].Count != 1)
        {
            throw new SolvingException();
        }

        var nodes = new Dictionary<string, BinaryTreeNode>(allGroupsOfLines[1].Count);

        foreach (var line in allGroupsOfLines[1])
        {
            var nodeId = _nodeRegex.Match(line).Groups["Node"].Value;
            var left = _nodeRegex.Match(line).Groups["Left"].Value;
            var right = _nodeRegex.Match(line).Groups["Right"].Value;

            if (!nodes.TryGetValue(nodeId, out var node))
            {
                node = new BinaryTreeNode(nodeId);
                nodes[nodeId] = node;
            }

            if (!nodes.TryGetValue(left, out var leftNode))
            {
                leftNode = new BinaryTreeNode(left);
                nodes[left] = leftNode;
            }

            if (!nodes.TryGetValue(right, out var rightNode))
            {
                rightNode = new BinaryTreeNode(right);
                nodes[right] = rightNode;
            }

            node.Left = leftNode;
            node.Right = rightNode;
        }

        return (allGroupsOfLines[0][0], nodes[Start]);
    }
}
