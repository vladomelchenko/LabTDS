using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LabTDS.Models;

namespace LabTDS
{
    public class ResolutionSolver
    {
        public SolverResult Solve(string input)
        {
            var disjunctionItems = this.ParseInput(input);
            var res = new SolverResult();

            res.Steps.AddRange(disjunctionItems.Select(el => el.ToString()));

            var solve = true;

            var appliedDisjinctions = new List<List<int>>();
            while (solve)
            {
                solve = this.TryFindDisjintion(disjunctionItems, appliedDisjinctions);
                if (solve)
                {
                    var lastClause = disjunctionItems[disjunctionItems.Count - 1];
                    var clauseStr = lastClause.ToString();
                    var lastApplied = appliedDisjinctions[appliedDisjinctions.Count - 1];
                    res.Steps.Add($"{clauseStr} ({lastApplied[0] + 1}, {lastApplied[1] + 1})");

                    if (lastClause.Stmts.Count == 0)
                    {
                        break;
                    }
                }
            }

            res.Items = disjunctionItems;

            return res;
        }

        private List<DisjunctionItem> ParseInput(string input)
        {
            var spl = input.Split('\n').Select(el => el.Trim('\r')).ToList();
            var res = new List<DisjunctionItem>();
            int idx = 0;
            foreach (var el in spl)
            {
                var strStmts = el.Split(',');
                var disjunctionItem = new DisjunctionItem();
                disjunctionItem.Index = ++idx;
                disjunctionItem.Stmts = strStmts.Select(s => new SimpleStatement(s.Trim())).ToList();

                res.Add(disjunctionItem);
            }

            return res;
        }

        private bool IsApplied(List<List<int>> appliedDisjunctions, int i, int j)
        {
            return appliedDisjunctions.Any(el =>
            {
                return el[0] == i && el[1] == j || el[1] == i && el[0] == j;
            });
        }

        private bool TryFindDisjintion(List<DisjunctionItem> clauses, List<List<int>> appliedDisjunctions)
        {
            for (int i = 0; i < clauses.Count - 1; i++)
            {
                for (int j = i + 1; j < clauses.Count; j++)
                {
                    if (!this.IsApplied(appliedDisjunctions, i, j) && clauses[i].CanDisjunct(clauses[j]))
                    {
                        appliedDisjunctions.Add(new List<int>() {i, j});
                        clauses.Add(clauses[i].Disjunct(clauses[j], clauses.Count + 1));
                        return true;
                    }
                }
            }

            return false;
        }
    }

    public class SolverResult
    {
        public List<DisjunctionItem> Items { get; set; } = new List<DisjunctionItem>();
        public List<string> Steps { get; set; } = new List<string>();
    }
}