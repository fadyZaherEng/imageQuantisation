using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
namespace ImageQuantization
{
    public struct colorProb
    {
        public double distance;
        public int colour1, colour2;
    }
    public class minHeap
    {
        public int count = 0;
        double distance;
        int colour1, colour2;
        public minHeap() { }
        public minHeap(colorProb color)
        {
            this.distance = color.distance;
            this.colour1 = color.colour1;
            this.colour2 = color.colour2;
        }
        private List<colorProb> ListOfColors = new List<colorProb>();

        public int GetParentIndex(int index)
        {
            return (index - 1) / 2;
        }
        public int GetLeftChildIndex(int index)
        {
            return 2 * index + 1;

        }
        public int GetRightChildIndex(int index)
        {
            return 2 * index + 2;

        }

        public bool HasLeftChild(int index)
        {
            return GetLeftChildIndex(index) < count;
        }
        public bool HasRightChild(int index)
        {
            return GetRightChildIndex(index) < count;
        }
        public bool IsRoot(int index)
        {
            return index == 0;
        }

        public double GetLeftChild(int index)
        {
            return ListOfColors[GetLeftChildIndex(index)].distance;
        }
        public double GetRightChild(int index)
        {
            return ListOfColors[GetRightChildIndex(index)].distance;
        }
        public double GetParent(int index)
        {
            return ListOfColors[GetParentIndex(index)].distance;
        }
        public void Swap(int firstIndex, int secondIndex)
        {
            var temp = ListOfColors[firstIndex];
            ListOfColors[firstIndex] = ListOfColors[secondIndex];
            ListOfColors[secondIndex] = temp;
        }
        public bool IsEmpty()
        {
            return count == 0;
        }
        public colorProb Peek()
        {
            return ListOfColors[0];
        }
        public colorProb pop()
        {
            var res = ListOfColors[0];
            ListOfColors[0] = ListOfColors[count - 1];
            count--;
            this.heapfydowen();
            return res;
        }
        public void push(colorProb item)
        {
            count++;
            ListOfColors.Add(item);
            this.heapfyup();

        }
        public void heapfydowen()
        {
            int index = 0;
            while (HasLeftChild(index))
            {
                var smallerIndex = GetLeftChildIndex(index);
                if (HasRightChild(index) && GetRightChild(index) < GetLeftChild(index))
                {
                    smallerIndex = GetRightChildIndex(index);
                }
                if (ListOfColors[smallerIndex].distance >= ListOfColors[index].distance)
                {
                    break;
                }
                Swap(smallerIndex, index);
                index = smallerIndex;
            }

        }


        private void heapfyup()
        {
            var index = count - 1;
            while (!IsRoot(index) && ListOfColors[index].distance < GetParent(index))
            {
                var parentIndex = GetParentIndex(index);
                Swap(parentIndex, index);
                index = parentIndex;
            }
        }
        public int size()
        {
            return count;
        }
    }
}
