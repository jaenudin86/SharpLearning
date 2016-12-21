﻿using MathNet.Numerics.LinearAlgebra;
using SharpLearning.Neural.Activations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SharpLearning.Neural.Layers
{
    /// <summary>
    /// Max pool layer
    /// </summary>
    [Serializable]
    public sealed class MaxPoolLayer : ILayer
    {
        /// <summary>
        /// 
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Depth { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Activation ActivationFunc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int InputHeight;

        /// <summary>
        /// 
        /// </summary>
        public int InputWidth;

        /// <summary>
        /// 
        /// </summary>
        public int InputDepth;

        int m_padding;
        int m_stride;

        int m_poolWidth;
        int m_poolHeight;

        /// <summary>
        /// Switches for determining the position of the max during forward and back propagation.
        /// </summary>
        public int[][] Switchx;

        /// <summary>
        /// Switches for determining the position of the max during forward and back propagation.
        /// </summary>
        public int[][] Switchy;

        /// <summary>
        /// 
        /// </summary>
        public Matrix<float> OutputActivations;

        Matrix<float> m_inputActivations;
        Matrix<float> m_delta;

        /// <summary>
        /// Max pool layer. 
        /// </summary>
        /// <param name="poolWidth">The width of the pool area</param>
        /// <param name="poolHeight">The height of the pool area</param>
        /// <param name="stride">Controls the distance between each neighbouring pool areas (default is 2)</param>
        public MaxPoolLayer(int poolWidth, int poolHeight, int stride=2)
        {
            m_poolWidth = poolWidth;
            m_poolHeight = poolHeight;
            m_stride = stride;
            m_padding = 0;
            ActivationFunc = Activation.Undefined;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="delta"></param>
        /// <returns></returns>
        public Matrix<float> Backward(Matrix<float> delta)
        {
            // enumerate each batch item one at a time
            Parallel.For(0, delta.RowCount, i =>
            {
                BackwardSingleItem(delta, m_delta, i);
            });

            return m_delta;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Matrix<float> Forward(Matrix<float> input)
        {
            m_inputActivations = input;

            // enumerate each batch item one at a time
            Parallel.For(0, input.RowCount, i =>
            {
                ForwardSingleItem(input, OutputActivations, i);
            });

            return OutputActivations;
        }

        void ForwardSingleItem(Matrix<float> input, Matrix<float> output, int batchItem)
        {
            var batchSize = input.RowCount;
            var inputData = input.Data();
            var outputData = output.Data();

            for (int depth = 0; depth < InputDepth; ++depth)
            {
                var n = depth * this.Width * this.Height; // a counter for switches
                var inputDepthOffSet = depth * InputHeight * InputWidth;
                var outputDeptOffSet = depth * Height * Width;

                for (int ph = 0; ph < Height; ++ph)
                {
                    var poolRowOffSet = ph * Width;
                    for (int pw = 0; pw < Width; ++pw)
                    {
                        int hstart = ph * m_stride - m_padding;
                        int wstart = pw * m_stride - m_padding;
                        int hend = Math.Min(hstart + m_poolHeight, InputHeight);
                        int wend = Math.Min(wstart + m_poolWidth, InputWidth);
                        hstart = Math.Max(hstart, 0);
                        wstart = Math.Max(wstart, 0);

                        var currentMax = float.MinValue;
                        int winx = -1, winy = -1;

                        for (int h = hstart; h < hend; ++h)
                        {
                            var rowOffSet = h * InputWidth;
                            for (int w = wstart; w < wend; ++w)
                            {
                                var inputColIndex = rowOffSet + w + inputDepthOffSet;
                                var inputIndex = inputColIndex * batchSize + batchItem;

                                var v = inputData[inputIndex];

                                // perform max pooling and store pointers to where
                                // the max came from. This will speed up backprop 
                                // and can help make nice visualizations in future
                                if (v > currentMax)
                                {
                                    currentMax = v;
                                    winx = w;
                                    winy = h;
                                }
                            }
                        }

                        this.Switchx[batchItem][n] = winx;
                        this.Switchy[batchItem][n] = winy;
                        n++;

                        var outputColIndex = poolRowOffSet + pw + outputDeptOffSet;
                        var outputIndex = outputColIndex * output.RowCount + batchItem;
                        outputData[outputIndex] = currentMax;                      
                    }
                }
            }
        }
        
        void BackwardSingleItem(Matrix<float> inputGradient, Matrix<float> outputGradient, int batchItem)
        {
            var batchSize = inputGradient.RowCount;
            var inputData = inputGradient.Data();
            var outputData = outputGradient.Data();

            var switchx = Switchx[batchItem];
            var switchy = Switchy[batchItem];

            for (var depth = 0; depth < this.Depth; depth++)
            {
                var n = depth * this.Width * this.Height;
                var inputDepthOffSet = depth * InputHeight * InputWidth;
                var outputDeptOffSet = depth * Height * Width;

                var x = -this.m_padding;
                var y = -this.m_padding;
                for (var ax = 0; ax < this.Width; x += this.m_stride, ax++)
                {
                    y = -this.m_padding;
                    var axOffSet = ax + outputDeptOffSet;
                    for (var ay = 0; ay < this.Height; y += this.m_stride, ay++)
                    {
                        var inputGradientColIndex = ay * Width + axOffSet;
                        var inputGradientIndex = inputGradientColIndex * batchSize + batchItem;
                        var chainGradient = inputData[inputGradientIndex];

                        var outputGradientColIndex = switchy[n] * InputWidth + switchx[n] + inputDepthOffSet;
                        var outputGradientIndex = outputGradientColIndex * outputGradient.RowCount + batchItem;
                        outputData[outputGradientIndex] = chainGradient;
                        n++;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parametersAndGradients"></param>
        public void AddParameresAndGradients(List<ParametersAndGradients> parametersAndGradients)
        {
            // Pool layer does not have any parameters or graidents.
        }

        /// <summary>
        /// Pool layer does not have any parameters or graidents.
        /// </summary>
        /// <returns></returns>
        public WeightsAndBiases GetGradients()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Pool layer does not have any parameters or graidents.
        /// </summary>
        /// <returns></returns>
        public WeightsAndBiases GetParameters()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputWidth"></param>
        /// <param name="inputHeight"></param>
        /// <param name="inputDepth"></param>
        /// <param name="batchSize"></param>
        /// <param name="random"></param>
        public void Initialize(int inputWidth, int inputHeight, int inputDepth, int batchSize, Random random)
        {
            InputWidth = inputWidth;
            InputHeight = inputHeight;
            InputDepth = inputDepth;

            // computed
            this.Depth = this.InputDepth;
            this.Width = ConvUtils.GetFilterGridLength(InputWidth, m_poolWidth, m_stride, m_padding);
            this.Height = ConvUtils.GetFilterGridLength(InputHeight, m_poolHeight, m_stride, m_padding);

            // store switches for x,y coordinates for where the max comes from, for each output neuron
            this.Switchx = Enumerable.Range(0, batchSize).Select(v => new int[this.Width * this.Height * this.Depth]).ToArray();
            this.Switchy = Enumerable.Range(0, batchSize).Select(v => new int[this.Width * this.Height * this.Depth]).ToArray();

            var fanIn = InputWidth * InputDepth * InputHeight;
            var fanOut = Depth * Width * Height;

            OutputActivations = Matrix<float>.Build.Dense(batchSize, fanOut);
            m_delta = Matrix<float>.Build.Dense(batchSize, fanIn);
        }

        /// <summary>
        /// Copies a minimal version of the layer to be used in a model for predictions.
        /// </summary>
        /// <param name="layers"></param>
        public void CopyLayerForPredictionModel(List<ILayer> layers)
        {
            var batchSize = 1;
            var copy = new MaxPoolLayer(m_poolWidth, m_poolHeight, m_stride);

            copy.InputDepth = InputDepth;
            copy.InputWidth = InputWidth;
            copy.InputHeight = InputHeight;

            copy.Depth = this.Depth;
            copy.Width = this.Width;
            copy.Height = this.Height;

            copy.Switchx = Enumerable.Range(0, batchSize).Select(v => new int[this.Width * this.Height * this.Depth]).ToArray();
            copy.Switchy = Enumerable.Range(0, batchSize).Select(v => new int[this.Width * this.Height * this.Depth]).ToArray();

            var fanOut = Width * Height * Depth;
            copy.OutputActivations = Matrix<float>.Build.Dense(batchSize, fanOut);

            layers.Add(copy);
        }
    }
}
