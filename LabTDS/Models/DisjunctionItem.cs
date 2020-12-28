using System.Collections.Generic;
using System.Linq;

namespace LabTDS.Models
{
    public class DisjunctionItem
    {
        public List<SimpleStatement> Stmts { get; set; }
        public int Index { get; set; }

        public bool CanDisjunct(DisjunctionItem item)
        {
            foreach (var stmt in Stmts)
            {
                foreach (var anotherStmt in item.Stmts)
                {
                    if (stmt.CanResolve(anotherStmt))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public DisjunctionItem Disjunct(DisjunctionItem another, int index)
        {
            var res = new DisjunctionItem()
            {
                Stmts = new List<SimpleStatement>(),
                Index = index
            };

            var cur = Stmts.ToList();
            var anotherStmts = another.Stmts.ToList();
            var allStmts = cur.Union(anotherStmts).ToList();

            var resolved = new HashSet<SimpleStatement>();
            {
                for (int i = 0; i < allStmts.Count - 1; i++)
                {
                    for (int j = 0; j < allStmts.Count; j++)
                    {
                        if (resolved.Contains(allStmts[i]) || resolved.Contains(allStmts[j]))
                        {
                            continue;
                        }

                        if (allStmts[i].CanResolve(allStmts[j]))
                        {
                            resolved.Add(allStmts[i]);
                            resolved.Add(allStmts[j]);
                            break;
                        }
                    }
                }

                var all = allStmts.ToList();
                foreach (var simpleStatement in all)
                {
                    if (resolved.Contains(simpleStatement))
                    {
                        allStmts.Remove(simpleStatement);
                    }
                }

                resolved.Clear();
            }

            {
                for (int i = 0; i < allStmts.Count - 1; i++)
                {
                    for (int j = 0; j < allStmts.Count; j++)
                    {
                        if (resolved.Contains(allStmts[i]) || resolved.Contains(allStmts[j]))
                        {
                            continue;
                        }

                        if (allStmts[i].CanResolve(allStmts[j]))
                        {
                            resolved.Add(allStmts[j]);
                            break;
                        }
                    }
                }

                var all = allStmts.ToList();
                foreach (var simpleStatement in all)
                {
                    if (resolved.Contains(simpleStatement))
                    {
                        allStmts.Remove(simpleStatement);
                    }
                }

                resolved.Clear();
            }

            res.Stmts = allStmts;
            return res;
        }

        public bool Equals(DisjunctionItem clause)
        {
            if (this.Stmts.Count != clause.Stmts.Count)
            {
                return false;
            }

            var thisHs = this.Stmts.ToHashSet();
            var otherHs = clause.Stmts.ToHashSet();

            foreach (var simpleStatement in thisHs)
            {
                if (!otherHs.Contains(simpleStatement))
                {
                    return false;
                }
            }

            foreach (var simpleStatement in otherHs)
            {
                if (!thisHs.Contains(simpleStatement))
                {
                    return false;
                }
            }

            return true;
        }

        public override string ToString()
        {
            if (this.Stmts.Count == 0)
            {
                return $"{this.Index}) 0";
            }

            return $"{this.Index}) {string.Join(" V ", this.Stmts.Select(el => el.ToString()))}";
        }
    }
}