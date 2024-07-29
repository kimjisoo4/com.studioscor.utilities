using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StudioScor.Utilities
{
    public static class TaskSystemUtilities
    {
        #region Task Action Decorator
        public static ITaskActionDecorator[] CloneTask(this IEnumerable<ITaskActionDecorator> originalTasks)
        {
            int count = originalTasks.Count();
            var cloneTasks = new ITaskActionDecorator[count];

            for (int i = 0; i < originalTasks.Count(); i++)
            {
                cloneTasks[i] = originalTasks.ElementAt(i).Clone();
            }

            return cloneTasks;
        }
        public static void Setup(this IEnumerable<ITaskActionDecorator> originalTasks, GameObject owner)
        {
            for (int i = 0; i < originalTasks.Count(); i++)
            {
                var originalTask = originalTasks.ElementAt(i);

                originalTask.Setup(owner);
            }
        }
        public static bool CheckAnyCondition(this IEnumerable<ITaskActionDecorator> originalTasks, GameObject target)
        {
            if (originalTasks is null)
                return true;

            int count = originalTasks.Count();

            if (count == 0)
                return true;

            for (int i = 0; i < count; i++)
            {
                var originalTask = originalTasks.ElementAt(i);

                if (originalTask.CheckCondition(target))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool CheckAllCondition(this IEnumerable<ITaskActionDecorator> originalTasks, GameObject target)
        {
            if (originalTasks is null)
                return true;

            int count = originalTasks.Count();

            if (count == 0)
                return true;

            for (int i = 0; i < count; i++)
            {
                var originalTask = originalTasks.ElementAt(i);

                if (!originalTask.CheckCondition(target))
                {
                    return false;
                }
            }

            return true;
        }
        #endregion

        #region Task Action

        public static ITaskAction[] CloneTask(this IEnumerable<ITaskAction> originalTasks)
        {
            int count = originalTasks.Count();
            var cloneTasks = new ITaskAction[count];

            for (int i = 0; i < originalTasks.Count(); i++)
            {
                cloneTasks[i] = originalTasks.ElementAt(i).Clone();
            }

            return cloneTasks;
        }
        public static void Setup(this IEnumerable<ITaskAction> originalTasks, GameObject owner)
        {
            for (int i = 0; i < originalTasks.Count(); i++)
            {
                var originalTask = originalTasks.ElementAt(i);

                originalTask.Setup(owner);
            }
        }
        public static void Action(this IEnumerable<ITaskAction> originalTasks, GameObject target)
        {
            for (int i = 0; i < originalTasks.Count(); i++)
            {
                var originalTask = originalTasks.ElementAt(i);

                originalTask.Action(target);
            }
        }
        #endregion

        #region Trace Task Action
        public static void Setup(this IEnumerable<ITraceTaskAction> originalTasks, GameObject owner)
        {
            for (int i = 0; i < originalTasks.Count(); i++)
            {
                var originalTask = originalTasks.ElementAt(i);

                originalTask.Setup(owner);
            }
        }
        public static ITraceTaskAction[] CloneTask(this IEnumerable<ITraceTaskAction> originalTasks)
        {
            int count = originalTasks.Count();
            var cloneTasks = new ITraceTaskAction[count];

            for (int i = 0; i < originalTasks.Count(); i++)
            {
                cloneTasks[i] = originalTasks.ElementAt(i).Clone() as ITraceTaskAction;
            }

            return cloneTasks;
        }

        #endregion
        public static IMainTask CloneTask(this IMainTask mainTask)
        {
            return mainTask.Clone() as IMainTask;
        }

        public static void OnTask(this IEnumerable<ISubTask> subTasks)
        {
            for (int i = 0; i < subTasks.Count(); i++)
            {
                var task = subTasks.ElementAt(i);

                task.OnTask();
            }
        }
        public static void CancelTask(this IEnumerable<ISubTask> subTasks)
        {
            for (int i = 0; i < subTasks.Count(); i++)
            {
                var task = subTasks.ElementAt(i);

                task.CancelTask();
            }
        }
        public static void UpdateSubTask(this IEnumerable<ISubTask> subTasks, float deltaTime, float normalizedTime)
        {
            for (int i = 0; i < subTasks.Count(); i++)
            {
                var task = subTasks.ElementAt(i);

                task.UpdateSubTask(deltaTime, normalizedTime);
            }
        }
        public static void FixedUpdateSubTask(this IEnumerable<ISubTask> subTasks, float deltaTime, float normalizedTime)
        {
            for (int i = 0; i < subTasks.Count(); i++)
            {
                var task = subTasks.ElementAt(i);

                task.FixedUpdateSubTask(deltaTime, normalizedTime);
            }
        }
        public static void ComplateTask(this IEnumerable<ISubTask> subTasks)
        {
            for (int i = 0; i < subTasks.Count(); i++)
            {
                var task = subTasks.ElementAt(i);

                task.ComplateTask();
            }
        }
        public static void Setup(this IEnumerable<ISubTask> subTasks, GameObject owner)
        {
            for(int i = 0; i < subTasks.Count(); i++)
            {
                var task = subTasks.ElementAt(i);
                
                task.Setup(owner);
            }
        }
        public static ISubTask[] CloneTask(this IEnumerable<ISubTask> originalTasks)
        {
            int count = originalTasks.Count();
            var cloneTasks = new ISubTask[count];

            for(int i = 0; i < originalTasks.Count(); i++)
            {
                cloneTasks[i] = originalTasks.ElementAt(i).Clone() as ISubTask;
            }

            return cloneTasks;
        }

    }

    public class TaskMachine
    {
        private readonly GameObject _owner;
        private readonly IMainTask _mainTask;
        private readonly ISubTask[] _subTasks;

        private bool isPlaying;
        public bool IsPlaying => isPlaying;

        public TaskMachine(GameObject owner,IMainTask mainTask, IEnumerable<ISubTask> subTasks)
        {
            _owner = owner;

            _mainTask = mainTask.CloneTask();
            _mainTask.Setup(_owner);

            _subTasks = subTasks.CloneTask();
            _subTasks.Setup(_owner);
        }

        public void OnTask()
        {
            if (isPlaying)
                return;

            isPlaying = true;

            _mainTask.OnTask();
            _subTasks.OnTask();
        }

        public void CancelTask()
        {
            if (!isPlaying)
                return;

           isPlaying = false;

            _mainTask.CancelTask();
            _subTasks.CancelTask();
        }

        public void ComplateTask()
        {
            if (!isPlaying)
                return;

            isPlaying = false;

            _mainTask.ComplateTask();
            _subTasks.ComplateTask();
        }

        public void UpdateTask(float deltaTime)
        {
            if (!isPlaying)
                return;

            _mainTask.UpdateTask(deltaTime);

            float normalizedTime = _mainTask.NormalizedTime;

            _subTasks.UpdateSubTask(deltaTime, normalizedTime);

            if (!_mainTask.IsPlaying)
                ComplateTask();
        }
        public void FixedUpdateTask(float deltaTime)
        {
            if (!isPlaying)
                return;

            _mainTask.FixedUpdateTask(deltaTime);

            float normalizedTime = _mainTask.NormalizedTime;

            _subTasks.FixedUpdateSubTask(deltaTime, normalizedTime);

            if (!_mainTask.IsPlaying)
                ComplateTask();
        }
    }

    public interface ITask
    {
        public bool IsPlaying { get; }
        public GameObject Owner { get; }

        public void Setup(GameObject owner);
        public ITask Clone();

        public void OnTask();
        public void CancelTask();
        public void ComplateTask();
    }
    public interface ISubTask : ITask
    {
        public void UpdateSubTask(float deltaTime, float normalizedTime);
        public void FixedUpdateSubTask(float deltaTime, float normalizedTime);
    }
    public interface IMainTask : ITask
    {
        public float NormalizedTime { get; }
        public void UpdateTask(float deltaTime);
        public void FixedUpdateTask(float deltaTime);
    }

    [Serializable]
    public abstract class Task : ITask
    {
        private GameObject _owner;
        private bool _isPlaying = false;

        public GameObject Owner => _owner;
        public bool IsPlaying => _isPlaying;

        public void Setup(GameObject owner)
        {
            _owner = owner;

            SetupTask();
        }

        public abstract ITask Clone();

        public void OnTask()
        {
            if (IsPlaying)
                return;

            _isPlaying = true;

            EnterTask();
        }
        public void CancelTask()
        {
            if (!IsPlaying)
                return;

            _isPlaying = false;

            OnCancelTask();

            ExitTask();
        }
        public void ComplateTask()
        {
            if (!IsPlaying)
                return;

            _isPlaying = false;

            OnComplateTask();

            ExitTask();
        }

        protected virtual void SetupTask()
        {

        }
        protected virtual void EnterTask()
        {

        }

        protected virtual void OnCancelTask()
        {

        }
        protected virtual void OnComplateTask()
        {

        }

        protected virtual void ExitTask()
        {

        }
    }

}
