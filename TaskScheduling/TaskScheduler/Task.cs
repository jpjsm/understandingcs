using System;
using System.Collections.Generic;
using System.Linq;

namespace STasks
{
    public class SComplexity
    {
        public int Index { get; set; }
        public int Complexity { get; set; }

        public SComplexity(int i, int c)
        {
            Index = i;
            Complexity = c;
        }
    }
    
    public class STask<TaskId> where TaskId: struct
    {
        public TaskId Id {get; private set;}
        public List<TaskId> Dependencies { get; private set; }

        public STask(TaskId id)
        {
            Id = id;
            Dependencies = new List<TaskId>();
        }

        public STask(TaskId id, IList<TaskId> dependencies)
        {
            if (dependencies == null)
            {
                throw new ArgumentNullException("dependencies");
            }

            if (dependencies.Any(t => t.Equals(id)))
            {
                throw new ArgumentException("dependencies", "A Task cannot depend on itself.");
            }

            Id = id;
            Dependencies = dependencies.ToList(); ;
        }

        public static TaskId[] TaskScheduling(IList<STask<TaskId>> taskbag)
        {
            List<TaskId> schedule = new List<TaskId>();

            int len = taskbag.Count;

            TaskId[] fromindex = new TaskId[len];
            Dictionary<TaskId, int> toindex = new Dictionary<TaskId, int>();


            int[,] taskdependency = new int[len,len];

            // build task dependency matrix and complexity vector
            List<SComplexity> complexity = new List<SComplexity>(); // Complexity of Index by Value
            int index = 0;
            foreach (STask<TaskId> task in taskbag)
            {
                
                if (!toindex.ContainsKey(task.Id))
                {
                    toindex.Add(task.Id, index);
                    fromindex[index++] = task.Id;
                }

                int complexvalue = 0;
                foreach (TaskId dependency in task.Dependencies)
                {
                    if (!toindex.ContainsKey(dependency))
                    {
                        toindex.Add(dependency, index);
                        fromindex[index++] = dependency;
                    }

                    taskdependency[toindex[task.Id],toindex[dependency]] = 1;
                    complexvalue++;
                }

                complexity.Add(new SComplexity(toindex[task.Id], complexvalue));
            }

            complexity = complexity.OrderBy(c => c.Complexity).ToList();

            int lastcomplexityreviewed = 0;

            // skip all zero complexity
            while (lastcomplexityreviewed < len && complexity[lastcomplexityreviewed].Complexity == 0)
            {
                lastcomplexityreviewed++;
            }


            // Add needed dependencies
            while (lastcomplexityreviewed < len)
            {
                bool nochange = true;
                int taskindex = complexity[lastcomplexityreviewed].Index;

                for (int i = 0; i < len; i++)
                {
                    if (taskdependency[taskindex, i] == 0) continue;

                    int dependentindex = i;
                    for (int j = 0; j < len; j++)
                    {
                        if (taskdependency[dependentindex, j] == 0 ||
                            taskdependency[taskindex, j] == 1) continue;

                        taskdependency[taskindex, j] = 1;
                        complexity[lastcomplexityreviewed].Complexity++;
                        nochange = false;
                    }
                }

                if (nochange)
                {
                    lastcomplexityreviewed++;
                }
                else
                {
                    complexity = complexity
                                    .Take(lastcomplexityreviewed)
                                    .Concat(complexity.Skip(lastcomplexityreviewed).OrderBy(c => c.Complexity))
                                    .ToList();
                }
            }

            return complexity.Select(c => fromindex[c.Index]).ToArray();
        }
    }
}
