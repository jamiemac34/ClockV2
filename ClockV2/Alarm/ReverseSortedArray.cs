using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PriorityQueue;

namespace ClockV2.Alarm
{
    public class ReverseSortedArray<T> : PriorityQueue<T>
    {
        private readonly PriorityItem<T>[] storage;
        private readonly int capacity;
        private int tailIndex;

        public ReverseSortedArray(int size)
        {
            storage = new PriorityItem<T>[size];
            capacity = size;
            tailIndex = -1;
        }

        public T Head()
        {
            if (IsEmpty())
            {
                throw new QueueUnderflowException();
            }
            return storage[0].Item;
        }

        public void Add(T item, int priority)
        {
            tailIndex++;
            if (tailIndex >= capacity)
            {
                tailIndex--;
                throw new QueueOverflowException();
            }

            int i = tailIndex;
            while (i > 0 && storage[i - 1].Priority > priority)
            {
                storage[i] = storage[i - 1];
                i--;
            }

            storage[i] = new PriorityItem<T>(item, priority);
        }

        public void Remove()
        {
            if (IsEmpty())
            {
                throw new QueueUnderflowException();
            }

            for (int i = 0; i < tailIndex; i++)
            {
                storage[i] = storage[i + 1];
            }
            tailIndex--;
        }

        public bool IsEmpty()
        {
            return tailIndex < 0;
        }

        public override string ToString()
        {
            if (IsEmpty())
            {
                throw new QueueUnderflowException("No items to display");
            }

            string result = "[";
            for (int i = 0; i <= tailIndex; i++)
            {
                if (i > 0)
                {
                    result += ", ";
                }
                result += storage[i];
            }
            result += "]";
            return result;
        }

        public bool Contains(T item)
        {
            if (tailIndex == -1)
            {
                return false;
            }

            int i = tailIndex;
            while (i >= 0)
            {
                if (storage[i].Item.Equals(item))
                {
                    return true;
                }
                else
                {
                    i--;
                }
                
            }
            return false;
        }

        public void populateList(ListBox lbAlarms)
        {
            int i = tailIndex;
            while (i >= 0)
            {
                lbAlarms.Items.Add(storage[i].Item.ToString());
                i--;
            }
        }

        public void removeViaIndex(int index)
        {
            for (int i = index; i < tailIndex; i++)
            {
                storage[i] = storage[i + 1];
            }
            tailIndex--;
        }
    }
}
