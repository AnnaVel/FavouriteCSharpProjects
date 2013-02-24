using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Second
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> contents = new List<int> { 3, 1, 2, 4, 7, 5, 6 };
            PriorityQueue<int> test = new PriorityQueue<int>(contents, 10);

            Console.WriteLine(test.RemoveMinimalElement());

        }
    }

   /// <summary>
    /// A simple implementation of a Priority queue through a min binary heap. It has two methods: Add element and remove the minimal element.
    /// </summary>
    /// <typeparam name="T">The element has to implement IComparable</typeparam>
    class PriorityQueue<T>
        where T : IComparable<T>
    {
        //properties
        private T[] heapArray;
        private int currentCapacity = 0;
        private const int DEFAULT_CAPACITY = 10;
        private int count;

        public int Count
        {
            get { return this.count; }
            set { this.count = value; }
        }

        #region Constructors

        public PriorityQueue(IEnumerable<T> collection, int initialCapacity)
        {
            if (collection.Count<T>() > initialCapacity)
            {
                throw new ArgumentException("The number of items in the collection is greater than the specified initial capacity");
            }

            this.currentCapacity = initialCapacity;
            heapArray = new T[currentCapacity];
            count = 0;
            foreach (T element in collection)
            {
                this.Add(element);
            }

        }

        public PriorityQueue(int initialCapacity)
        {
            this.currentCapacity = initialCapacity;
            heapArray = new T[initialCapacity];
            count = 0;
        }

        public PriorityQueue()
        {
            heapArray = new T[DEFAULT_CAPACITY];
            this.currentCapacity = DEFAULT_CAPACITY;
            count = 0;
        }

        #endregion

        #region Methods

        public void Add(T element)
        {
            if (element == null)
            {
                throw new ArgumentException("Element cannot be null");
            }

            if (count >= currentCapacity)
            {
                T[] newArray = new T[currentCapacity * 2];
                for (int i = 0; i < count; i++)
                {
                    newArray[i] = heapArray[i];
                }
                heapArray = newArray;
                newArray = null;
                currentCapacity *= 2;
            }

            heapArray[count] = element;
            int currentPosition = count;
            while (heapArray[currentPosition].CompareTo(heapArray[(currentPosition - 1) / 2]) < 0)
            {
                this.SwapTwoElements(currentPosition, (currentPosition - 1) / 2);
                currentPosition = (currentPosition - 1) / 2;
            }
            count++;
        }

        public T RemoveMinimalElement()
        {
            T result = heapArray[0];
            heapArray[0] = heapArray[count - 1];
            heapArray[count - 1] = default(T);
            count--;

            int currentPosition = 0;
            int smallerChildPosition;
            if (count >= 2)
            {
                if (count > 2)
                {
                    smallerChildPosition = heapArray[currentPosition * 2 + 1].CompareTo(heapArray[currentPosition * 2 + 2]) < 0 ? 1 : 2;
                }
                else // count == 2 and the main node has just one child
                {
                    smallerChildPosition = 1;
                }

                while (heapArray[currentPosition].CompareTo(heapArray[currentPosition * 2 + smallerChildPosition]) > 0)
                {
                    this.SwapTwoElements(currentPosition, currentPosition * 2 + smallerChildPosition);
                    currentPosition = currentPosition * 2 + smallerChildPosition;
                    if (currentPosition * 2 + 1 >= count) // currentPosition * 2 + 2 >= count is automatically true = node has no child
                    {
                        break;
                    }
                    else if (currentPosition * 2 + 2 >= count) //node has one child
                    {
                        smallerChildPosition = 1;
                    }
                    else //the node has two children
                    {
                        smallerChildPosition = heapArray[currentPosition * 2 + 1].CompareTo(heapArray[currentPosition * 2 + 2]) < 0 ? 1 : 2;
                    }
                }
            }

            return result;
        }

        private void SwapTwoElements(int index1, int index2)
        {
            T help = heapArray[index1];
            heapArray[index1] = heapArray[index2];
            heapArray[index2] = help;
        }

        #endregion

    }
}
