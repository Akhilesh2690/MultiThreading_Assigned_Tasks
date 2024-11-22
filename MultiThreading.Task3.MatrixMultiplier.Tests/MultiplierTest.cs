using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultiThreading.Task3.MatrixMultiplier.Matrices;
using MultiThreading.Task3.MatrixMultiplier.Multipliers;
using System;
using System.Diagnostics;

namespace MultiThreading.Task3.MatrixMultiplier.Tests
{
    [TestClass]
    public class MultiplierTest
    {
        [TestMethod]
        public void MultiplyMatrix3On3Test()
        {
            TestMatrix3On3(new MatricesMultiplier());
            TestMatrix3On3(new MatricesMultiplierParallel());
            ParallelEfficiencyTest();
        }

        [TestMethod]
        public void ParallelEfficiencyTest()
        {
            int[] matrixSizes = new int[] { 10, 50, 100, 200 };

            foreach (var size in matrixSizes)
            {
                var matrix1 = GenerateMatrix(size);
                var matrix2 = GenerateMatrix(size);

                var regularMultiplier = new MatricesMultiplier();
                var regularTime = MeasureTime(() => regularMultiplier.Multiply(matrix1, matrix2));

                var parallelMultiplier = new MatricesMultiplierParallel();
                var parallelTime = MeasureTime(() => parallelMultiplier.Multiply(matrix1, matrix2));

                Console.WriteLine($"Matrix Size: {size}x{size}");
                Console.WriteLine($"Regular Multiplication Time: {regularTime} ms");
                Console.WriteLine($"Parallel Multiplication Time: {parallelTime} ms");

                if (size < 50)
                {
                    Assert.IsTrue(regularTime <= parallelTime,
                    $"For small matrices, regular multiplication should be faster or equal to parallel multiplication for size {size}x{size} ");
                }
                else
                {
                    Assert.IsTrue(parallelTime < regularTime,
                     $"Parallel multiplication should be faster for matrices of size {size}x{size} ");
                }
            }
        }

        #region private methods

        private void TestMatrix3On3(IMatricesMultiplier matrixMultiplier)
        {
            if (matrixMultiplier == null)
            {
                throw new ArgumentNullException(nameof(matrixMultiplier));
            }

            var m1 = new Matrix(3, 3);
            m1.SetElement(0, 0, 34);
            m1.SetElement(0, 1, 2);
            m1.SetElement(0, 2, 6);

            m1.SetElement(1, 0, 5);
            m1.SetElement(1, 1, 4);
            m1.SetElement(1, 2, 54);

            m1.SetElement(2, 0, 2);
            m1.SetElement(2, 1, 9);
            m1.SetElement(2, 2, 8);

            var m2 = new Matrix(3, 3);
            m2.SetElement(0, 0, 12);
            m2.SetElement(0, 1, 52);
            m2.SetElement(0, 2, 85);

            m2.SetElement(1, 0, 5);
            m2.SetElement(1, 1, 5);
            m2.SetElement(1, 2, 54);

            m2.SetElement(2, 0, 5);
            m2.SetElement(2, 1, 8);
            m2.SetElement(2, 2, 9);

            var multiplied = matrixMultiplier.Multiply(m1, m2);
            Assert.AreEqual(448, multiplied.GetElement(0, 0));
            Assert.AreEqual(1826, multiplied.GetElement(0, 1));
            Assert.AreEqual(3052, multiplied.GetElement(0, 2));

            Assert.AreEqual(350, multiplied.GetElement(1, 0));
            Assert.AreEqual(712, multiplied.GetElement(1, 1));
            Assert.AreEqual(1127, multiplied.GetElement(1, 2));

            Assert.AreEqual(109, multiplied.GetElement(2, 0));
            Assert.AreEqual(213, multiplied.GetElement(2, 1));
            Assert.AreEqual(728, multiplied.GetElement(2, 2));
        }

        #endregion private methods

        private Matrix GenerateMatrix(int size)
        {
            var matrix = new Matrix(size, size);
            var rand = new Random();
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    matrix.SetElement(i, j, rand.Next(1, 100));
                }
            }
            return matrix;
        }

        private long MeasureTime(Action action)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            action();
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }
    }
}