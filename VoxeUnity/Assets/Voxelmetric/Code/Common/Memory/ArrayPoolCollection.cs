﻿using System;
using System.Collections.Generic;

namespace Voxelmetric.Code.Common.Memory
{
    public class ArrayPoolCollection<T>
    {
        private readonly Dictionary<int, IArrayPool<T>> m_arrays;

        public ArrayPoolCollection(int size)
        {
            m_arrays = new Dictionary<int, IArrayPool<T>>(size);
        }

        public T[] Pop(int size)
        {
            int length = GetRoundedSize(size);

            IArrayPool<T> pool;
            if (!m_arrays.TryGetValue(length, out pool))
            {
                pool = new ArrayPool<T>(length, 4, 1);
                m_arrays.Add(length, pool);
            }

            return pool.Pop();
        }

        public void Push(T[] array)
        {
            int length = array.Length;

            IArrayPool<T> pool;
            if (!m_arrays.TryGetValue(length, out pool))
                throw new InvalidOperationException("Couldn't find an array pool of length " + length);

            pool.Push(array);
        }

        private static readonly int RoundSizeBy = 100;

        protected static int GetRoundedSize(int size)
        {
            int rounded = size / RoundSizeBy * RoundSizeBy;
            return rounded == size ? rounded : rounded + RoundSizeBy;
        }

        public override string ToString()
        {
            return m_arrays.Count.ToString();
        }
    }
}