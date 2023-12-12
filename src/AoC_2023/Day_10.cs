using SheepTools.Extensions;
using SheepTools.Model;
using Spectre.Console;

namespace AoC_2023;

public class Day_10 : BaseDay
{
    private sealed record PipeNode : IntPoint
    {
        public char Value { get; }

        public PipeNode? Parent { get; set; }

        public List<Direction> ConnectionsDirections { get; set; }

        public List<PipeNode> Connections { get; set; }

        public PipeNode(char value, int x, int y) : base(x, y)
        {
            Value = value;

            // Up is Y increment, so our 'down' if Y increments downwards
            ConnectionsDirections = value switch
            {
                '.' => [],
                '|' => [Direction.Down, Direction.Up],
                'L' => [Direction.Down, Direction.Right],
                'J' => [Direction.Down, Direction.Left],
                '7' => [Direction.Up, Direction.Left],
                'F' => [Direction.Up, Direction.Right],
                '-' => [Direction.Right, Direction.Left],
                'S' => new(2),
                _ => throw new SolvingException()
            };

            Connections = new(2);
        }

        public bool IsStart() => Value == 'S';

        public bool IsPipe() => Value != '.';

        public bool Equals(PipeNode? other)
        {
            return base.Equals(other);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    private readonly List<List<PipeNode>> _input;

    public Day_10()
    {
        _input = ParseInput().ToList();
    }

    public override ValueTask<string> Solve_1()
    {
        var start = FindStartPipeNodeAndAdjacents();

        // BFS
        var queue = new Queue<PipeNode>(_input.Count);
        var expanded = new Dictionary<PipeNode, List<PipeNode>>(_input.Count);

        PipeNode currentNode = start.Connections[0];
        currentNode.Parent = start;
        queue.Enqueue(currentNode);

        while (true)
        {
            currentNode = queue.Dequeue();
            var previousSolution = currentNode.Parent == start
                ? new(_input.Count)
                : expanded[currentNode.Parent!];

            var currentSolution = previousSolution.Append(currentNode).ToList();
            expanded.TryAdd(currentNode, currentSolution);

            if (currentNode == start)
            {
                break;
            }

            foreach (var neighbour in ExtractPipesNodesAroundExcludingPreviousOne(currentNode, currentNode.Parent!))
            {
                if (!expanded.ContainsKey(neighbour))
                {
                    neighbour.Parent = currentNode;
                    queue.Enqueue(neighbour);
                }
            }
        }

         int result = expanded[currentNode].Count / 2;

        // PrintPipes(expanded, currentNode);

        return new($"{result}");
    }

    private PipeNode FindStartPipeNodeAndAdjacents()
    {
        for (int y = 0; y < _input.Count; ++y)
        {
            for (int x = 0; x < _input.Count; ++x)
            {
                var pipeNode = _input[y][x];

                if (pipeNode.IsStart())
                {
                    pipeNode.Connections.AddRange(ExtractPipesNodesAround(pipeNode, lookingForStart: true));

                    pipeNode.ConnectionsDirections.AddRange(pipeNode.Connections
                        .Select(neighbour =>
                            neighbour.ConnectionsDirections.Single(d => neighbour.Move(d) == pipeNode).Opposite()));

                    return pipeNode;
                }
            }
        }

        throw new SolvingException();
    }

    private IEnumerable<PipeNode> ExtractPipesNodesAround(PipeNode pipeNode, bool lookingForStart)
    {
        return
            (new[]{
                pipeNode.Move(Direction.Up),
                pipeNode.Move(Direction.Down),
                pipeNode.Move(Direction.Left),
                pipeNode.Move(Direction.Right)})
            .Where(intPoint =>
                intPoint.X == Math.Clamp(intPoint.X, 0, _input[0].Count - 1)
                && intPoint.Y == Math.Clamp(intPoint.Y, 0, _input.Count - 1))
            .Select(intPoint => _input[intPoint.Y][intPoint.X])
            .Where(pipelineAround =>
                (lookingForStart && pipelineAround.IsStart())
                || pipelineAround.ConnectionsDirections.Any(d => pipelineAround.Move(d) == pipeNode));
    }

    private IEnumerable<PipeNode> ExtractPipesNodesAroundExcludingPreviousOne(PipeNode pipeNode, PipeNode previousOne)
    {
        return ExtractPipesNodesAround(pipeNode, lookingForStart: false)
            .Where(pipeNode => pipeNode != previousOne);
    }

    public override ValueTask<string> Solve_2()
    {
        int result = 0;

        return new($"{result}");
    }

    private IEnumerable<List<PipeNode>> ParseInput()
    {
        var file = new ParsedFile(InputFilePath);
        int y = 0;

        while (!file.Empty)
        {
            var line = file.NextLine();
            var returnLine = new List<PipeNode>(line.Count);

            int x = 0;
            foreach (var ch in line.ToList<char>())
            {
                returnLine.Add(new(ch, x++, y));
            }

            y++;
            yield return returnLine;
        }
    }

    private void PrintPipes(Dictionary<PipeNode, List<PipeNode>> expanded, PipeNode currentNode)
    {
        // int index = 0;

        var minY = expanded[currentNode].Min(x => x.Y);// - 3;
        var maxY = expanded[currentNode].Max(x => x.Y) + 1;
        var minX = expanded[currentNode].Min(x => x.X);// - 3;
        var maxX = expanded[currentNode].Max(x => x.X) + 1;
        for (int y = minY; y < maxY; ++y)
        {
            for (int x = minX; x < maxX; ++x)
            {
                if (expanded[currentNode].Contains(_input[y][x]))
                {
                    //Console.Write(index++);
                    //Console.Write(expanded[currentNode].IndexOf(_input[y][x]).ToString("000"));
                    Console.Write(_input[y][x].Value);
                }
                else
                {
                    Console.Write("...");
                }
                Console.Write(' ');
            }

            Console.WriteLine();
        }
    }
}
