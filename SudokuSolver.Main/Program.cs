using System;

namespace SudokuSolver;
class Program
{
    private static readonly List<int> _sudokuNumbers = new List<int>(){1,2,3,4,5,6,7,8,9};

    static void Main(string[] args)
    {
        var solutions = new List<string>();
        var puzzle =  @"0,0,0,4,9,5,0,0,0,
                       0,0,9,0,0,0,3,0,0,
                       0,2,0,0,0,0,0,8,0,
                       0,0,2,0,3,0,0,0,4,
                       0,0,0,2,0,8,0,0,0,
                       1,0,0,0,7,0,9,0,0,
                       0,7,0,0,0,0,0,2,0,
                       0,0,3,0,0,0,5,0,0,
                       0,0,0,7,6,1,0,0,0";

        string[] puzzleAsStringCollection = puzzle.Split(',');
        List<int> sudoku = new List<int>();
        foreach(var puzzleString in puzzleAsStringCollection){
            sudoku.Add(int.Parse(puzzleString));
        }

        Solve(sudoku, solutions);
        foreach(var solution in solutions){
            Console.WriteLine(solution);
        }
    }

    public static string[] GetCandidates(List<int> state)
    {
        
        var rows = GetRows(state);
        foreach(var row in rows){
            var random = new Random();
            var usedNumbers = row.Value.Where(x => x > 0);
            var availableNumbers = _sudokuNumbers.Except(usedNumbers).ToList();

            var attempt = new List<int>();

            foreach(var item in row.Value){
                int randomNumber = random.Next(0, availableNumbers.Count);
                if(item == 0){
                    attempt.Add(availableNumbers[randomNumber]);
                    availableNumbers.Remove(availableNumbers[randomNumber]);
                }else{
                    attempt.Add(item);
                }
            }

            Console.WriteLine($"Attempt: {string.Join(',', attempt)}");
        }
    }

    public static bool IsValid(List<int> state)
    {
        // Each row must contain all numbers from 1-9
        if(!ValidateRows(state)){
            return false;
        }

        // Each column must contain all numbers from 1-9
        // ValidateColumns(state)

        // Each grid must contain all numbers from 1-9
        // ValidateGrids(state)

        return true;
    }

    public static bool ValidateRows(List<int> state){
        var rowDictionary = new Dictionary<int, List<int>>();
        for (int i = 0; i < 9; i++)
        {
            var intArray = new List<int>();

            for (int j = 0; j < 9; j++){
                var index = i * 9 + j;
                var value = state.ElementAt(index);
                intArray.Add(value);
            }

            // Does the row contain all numbers 1-9
            foreach(var number in _sudokuNumbers){
                if(!intArray.Contains(number))
                    return false;
            }

            rowDictionary.Add(i, intArray);
        }

        // Validate that a row does not contain the same value as another in the same position (column)
        for (int i = 0; i < 9; i++)
        {
            int[] comparer = new int[]{
                rowDictionary[0].ElementAt(i),
                rowDictionary[1].ElementAt(i),
                rowDictionary[2].ElementAt(i),
                rowDictionary[3].ElementAt(i),
                rowDictionary[4].ElementAt(i),
                rowDictionary[5].ElementAt(i),
                rowDictionary[6].ElementAt(i),
                rowDictionary[7].ElementAt(i),
                rowDictionary[8].ElementAt(i),
            };

            var result = comparer.GroupBy(x => x).Where(g => g.Count() > 1).SelectMany(g => g).Any();
            if(result == true)
                return false;
        }

        return true;
    }

    
    public static void Solve(List<int> state, List<string> solutions)
    {
        if(IsValid(state)){
            solutions.Add(string.Join(',', state));
            return;
        }

        foreach(var candidate in GetCandidates(state)){
            Solve(possibleSolution, solutions)
        }      
    }

    public static Dictionary<int, List<int>> GetColumns(string[] sudokuGrid){
        var columnDictionary = new Dictionary<int, List<int>>();
        for (int i = 0; i < 9; i++)
        {
            var intArray = new List<int>();

            for (int j = 0; j < 9; j++){
                var index = i + j * 9;
                var value = int.Parse(sudokuGrid.ElementAt(index));
                intArray.Add(value);
            }

            columnDictionary.Add(i, intArray);
        }

        return columnDictionary;
    }

    public static Dictionary<int, List<int>> GetRows(List<int> state){
        var rowDictionary = new Dictionary<int, List<int>>();
        for (int i = 0; i < 9; i++)
        {
            var intArray = new List<int>();

            for (int j = 0; j < 9; j++){
                var index = i * 9 + j;
                var value = state.ElementAt(index);
                intArray.Add(value);
            }

            rowDictionary.Add(i, intArray);
        }

        return rowDictionary;
    }

    public static Dictionary<int, List<int>> GetGrids(Dictionary<int, List<int>> rowDictionary){
        var gridDictionary = new Dictionary<int, List<int>>();

        // i is grid number
        for (int i = 0; i < 9; i++)
        {      
            if(i < 3){
                var innerCollection = new List<int>();
                for (int m = 0; m < 3; m++)
                {
                    var row = rowDictionary[m];
                    innerCollection.AddRange(row.Skip(i * 3).Take(3));
                }

                gridDictionary.Add(i, innerCollection);
            }

            
            if(i >= 3 && i < 6){
                var innerCollection = new List<int>();
                for (int m = 3; m < 6; m++)
                {
                    var row = rowDictionary[m];
                    innerCollection.AddRange(row.Skip((i-3) * 3).Take(3));
                }
                gridDictionary.Add(i, innerCollection);
            }

            if(i >= 6 && i < 9){
                var innerCollection = new List<int>();
                for (int m = 6; m < 9; m++)
                {
                    var row = rowDictionary[m];
                    innerCollection.AddRange(row.Skip((i-6) * 3).Take(3));
                }
                gridDictionary.Add(i, innerCollection);
            }

        }

        return gridDictionary;
    }                      
}
