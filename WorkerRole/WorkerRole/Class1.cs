using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.ServiceRuntime;


namespace WorkerRole
{
    public abstract class WorkerTask
    {
        public virtual bool OnStart() { return true; }
        public virtual void Run() { }
        public virtual void OnStop() { }
    }

    public class TaskHandler : TasksRunner
    {
        private List<WorkerTask> tasks = new List<WorkerTask>();

        protected override IList<WorkerTask> Tasks
        {
            get { return tasks; }
        }
    }

    public abstract class TasksRunner : RoleEntryPoint
    {
        protected abstract IList<WorkerTask> Tasks { get; }
        private Queue<WorkerTask> waitingToStart = new Queue<WorkerTask>();
        private Queue<WorkerTask> waitingToRun = new Queue<WorkerTask>();
        private Queue<WorkerTask> failedToStart = new Queue<WorkerTask>();
        private List<WorkerTask> running = new List<WorkerTask>();
        private ManualResetEvent readyToStop = new ManualResetEvent(false);
        private bool isStopping = false;

        public override bool OnStart()
        {
            foreach (var task in Tasks)
            {
                waitingToStart.Enqueue(task);
            }

            return base.OnStart();
        }

        public override void OnStop()
        {
            isStopping = true;
            readyToStop.WaitOne();

            foreach (var task in running)
            {
                task.OnStop();
            }

            base.OnStop();
        }

        public override void Run()
        {
            base.Run();

            while (!isStopping)
            {
                while (waitingToStart.Count > 0)
                {
                    WorkerTask task = waitingToStart.Dequeue();

                    if (task.OnStart())
                    {
                        waitingToRun.Enqueue(task);
                    }
                    else
                    {
                        failedToStart.Enqueue(task);
                    }
                }

                while (failedToStart.Count > 0)
                {
                    waitingToStart.Enqueue(failedToStart.Dequeue());
                }

                while (waitingToRun.Count > 0)
                {
                    WorkerTask task = waitingToRun.Dequeue();
                    Task.Factory.StartNew(
                        () =>
                        {
                            try
                            {
                                running.Add(task);
                                task.Run();
                            }
                            catch (Exception ex)
                            {
                                Trace.TraceError("", ex);
                            }
                            finally
                            {
                                running.Remove(task);
                                waitingToStart.Enqueue(task);
                            }                             
                        },
                        TaskCreationOptions.LongRunning);
                }

                Thread.Sleep(15000); // wait 15 seconds before re-checking
            }

            readyToStop.Set();
        }
    }
}
