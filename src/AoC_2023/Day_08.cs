using System.Text.RegularExpressions;

namespace AoC_2023;

public partial class Day_08 : BaseDay
{
    [GeneratedRegex(@"(?<Node>\w*(?=\s=)).*(?<Left>(?<=\()\w*(?=,)).*(?<Right>(?<=\s)\w*(?=\)))\)")]
    private static partial Regex NodeRegex();

    private readonly Regex _nodeRegex = NodeRegex();

    private sealed record BinaryTreeNode
    {
        public string Id { get; }

        public BinaryTreeNode Left { get; set; } = null!;

        public BinaryTreeNode Right { get; set; } = null!;

        public bool IsEndNodePart2 { get; }

        public BinaryTreeNode(string id)
        {
            Id = id;
            IsEndNodePart2 = Id[^1] == 'Z';
        }
    }

    private readonly (string Instructions, Dictionary<string, BinaryTreeNode> Nodes) _input;

    public Day_08()
    {
        _input = ParseInput();
    }

    public override ValueTask<string> Solve_1()
    {
        const string start = "AAA";
        const string end = "ZZZ";

        int counter = 0;
        BinaryTreeNode currentNode = _input.Nodes[start];

        while (true)
        {
            currentNode = _input.Instructions[counter++ % _input.Instructions.Length] switch
            {
                'L' => currentNode.Left,
                'R' => currentNode.Right,
                _ => throw new SolvingException()
            };

            if (currentNode.Id == end)
            {
                return new($"{counter}");
            }
        }
    }

    public override ValueTask<string> Solve_2()
    {
        var startNodeList = new List<BinaryTreeNode>(_input.Nodes.Count);
        var endNodeList = new List<BinaryTreeNode>(_input.Nodes.Count);

        foreach (var node in _input.Nodes)
        {
            if (node.Key[^1] == 'A')
            {
                startNodeList.Add(node.Value);
            }
            else if (node.Key[^1] == 'Z')
            {
                endNodeList.Add(node.Value);
            }
        }

        var currentNodeArray = startNodeList.ToArray();
        var repetitionPeriodAfterEnd = new ulong[currentNodeArray.Length];

        ulong counter = 0;

        bool finish = false;
        while (!finish)
        {
            var instruction = _input.Instructions[(int)(counter++ % (ulong)_input.Instructions.Length)];
            finish = repetitionPeriodAfterEnd.All(p => p != default);

            for (int i = 0; i < currentNodeArray.Length; ++i)
            {
                currentNodeArray[i] = instruction switch
                {
                    'L' => currentNodeArray[i].Left,
                    'R' => currentNodeArray[i].Right,
                    _ => throw new SolvingException()
                };

                if (currentNodeArray[i].IsEndNodePart2
                    && (repetitionPeriodAfterEnd[i] == 0    // Initial iteration, to avoid dividing by 0
                        || (counter - repetitionPeriodAfterEnd[i]) % repetitionPeriodAfterEnd[i] != 0))
                {
                    repetitionPeriodAfterEnd[i] = counter - repetitionPeriodAfterEnd[i];
                    finish = false;
                }
            }
        }

        var result = SheepTools.Maths.LeastCommonMultiple(repetitionPeriodAfterEnd);

        return new($"{result}");
    }

    private (string, Dictionary<string, BinaryTreeNode>) ParseInput()
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

        return (allGroupsOfLines[0][0], nodes);
    }
}
