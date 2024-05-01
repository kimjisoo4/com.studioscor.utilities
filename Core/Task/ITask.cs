using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

namespace StudioScor.Utilities
{
    public static class TaskSystemUtilities
    {
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
        public static void UpdateSubTask(this IEnumerable<ISubTask> subTasks, float normalizedTime)
        {
            for (int i = 0; i < subTasks.Count(); i++)
            {
                var task = subTasks.ElementAt(i);

                task.UpdateSubTask(normalizedTime);
            }
        }
        public static void FixedUpdateSubTask(this IEnumerable<ISubTask> subTasks, float normalizedTime)
        {
            for (int i = 0; i < subTasks.Count(); i++)
            {
                var task = subTasks.ElementAt(i);

                task.FixedUpdateSubTask(normalizedTime);
            }
        }
        public static void EndTask(this IEnumerable<ISubTask> subTasks)
        {
            for (int i = 0; i < subTasks.Count(); i++)
            {
                var task = subTasks.ElementAt(i);

                task.EndTask();
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
        public void EndTask()
        {
            if (!isPlaying)
                return;

            isPlaying = false;

            _mainTask.EndTask();
            _subTasks.EndTask();
        }

        public void UpdateTask(float deltaTime)
        {
            if (!isPlaying)
                return;

            _mainTask.UpdateTask(deltaTime);

            float normalizedTime = _mainTask.NormalizedTime;

            _subTasks.UpdateSubTask(normalizedTime);

            if (!_mainTask.IsPlaying)
                EndTask();
        }
        public void FixedUpdateTask(float deltaTime)
        {
            if (!isPlaying)
                return;

            _mainTask.FixedUpdateTask(deltaTime);

            float normalizedTime = _mainTask.NormalizedTime;

            _subTasks.FixedUpdateSubTask(normalizedTime);

            if (!_mainTask.IsPlaying)
                EndTask();
        }


    }
    public interface ITask
    {
        public bool IsPlaying { get; }
        public GameObject Owner { get; }

        public void Setup(GameObject owner);
        public ITask Clone();

        public void OnTask();
        public void EndTask();
    }
    public interface ISubTask : ITask
    {
        public void UpdateSubTask(float normalizedTime);
        public void FixedUpdateSubTask(float normalizedTime);
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
        public void EndTask()
        {
            if (!IsPlaying)
                return;

            _isPlaying = false;

            ExitTask();
        }

        protected virtual void SetupTask()
        {

        }
        protected virtual void EnterTask()
        {

        }
        protected virtual void ExitTask()
        {

        }
    }

}
