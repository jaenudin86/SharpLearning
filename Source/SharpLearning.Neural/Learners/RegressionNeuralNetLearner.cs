﻿using SharpLearning.Common.Interfaces;
using SharpLearning.Containers.Matrices;
using SharpLearning.Neural.Layers;
using SharpLearning.Neural.Loss;
using SharpLearning.Neural.Models;
using SharpLearning.Neural.Optimizers;
using SharpLearning.Neural.TargetEncoders;
using System;
using System.Linq;

namespace SharpLearning.Neural.Learners
{
    /// <summary>
    /// RegressionNeuralNet learner using mini-batch gradient descent. 
    /// </summary>
    public sealed class RegressionNeuralNetLearner : IIndexedLearner<double>, ILearner<double>
    {
        readonly NeuralNetLearner m_learner;

        /// <summary>
        /// RegressionNeuralNet learner using mini-batch gradient descent. 
        /// Several optimization methods is availible through the constructor.
        /// </summary>
        /// <param name="net">The neural net to learn</param>
        /// <param name="loss">The loss measured and shown between each iteration</param>
        /// <param name="learningRate">Controls the step size when updating the weights. (Default is 0.01)</param>
        /// <param name="iterations">The maximum number of iterations before termination. (Default is 100)</param>
        /// <param name="batchSize">Batch size for mini-batch stochastic gradient descent. (Default is 128)</param>
        /// <param name="l1decay">L1 reguralization term. (Default is 0, so no reguralization)</param>
        /// <param name="l2decay">L2 reguralization term. (Default is 0, so no reguralization)</param>
        /// <param name="optimizerMethod">The method used for optimization (Default is Adagrad)</param>
        /// <param name="momentum">Momentum for gradient update. Should be between 0 and 1. (Defualt is 0.9)</param>
        /// <param name="ro"></param>
        /// <param name="beta1">Exponential decay rate for estimates of first moment vector, should be in range 0 to 1 (Default is 0.9)</param>
        /// <param name="beta2">Exponential decay rate for estimates of second moment vector, should be in range 0 to 1 (Default is 0.999)</param>
        public RegressionNeuralNetLearner(NeuralNet net, ILoss loss, double learningRate = 0.01, int iterations = 100, int batchSize = 128, double l1decay = 0, double l2decay = 0,
            OptimizerMethod optimizerMethod = OptimizerMethod.Adagrad, double momentum = 0.9, double ro = 0.95, double beta1 = 0.9, double beta2 = 0.999)
        {
            if (!(net.Layers.Last() is IRegressionLayer))
            {
                throw new ArgumentException("Last layer must be a regression layer type. Was: " + net.Layers.Last().GetType().Name);
            }

            m_learner = new NeuralNetLearner(net, new CopyTargetEncoder(), loss,
                learningRate, iterations, batchSize, l1decay, l2decay, optimizerMethod, momentum, ro, beta1, beta2);
        }

        /// <summary>
        /// Learns a regression neural network
        /// </summary>
        /// <param name="observations"></param>
        /// <param name="targets"></param>
        /// <returns></returns>
        public RegressionNeuralNetModel Learn(F64Matrix observations, double[] targets)
        {
            var model = m_learner.Learn(observations, targets);
            return new RegressionNeuralNetModel(model);
        }

        /// <summary>
        /// Learns a regression neural network
        /// </summary>
        /// <param name="observations"></param>
        /// <param name="targets"></param>
        /// <param name="indices"></param>
        /// <returns></returns>
        public RegressionNeuralNetModel Learn(F64Matrix observations, double[] targets, int[] indices)
        {
            var model = m_learner.Learn(observations, targets, indices);
            return new RegressionNeuralNetModel(model);
        }

        /// <summary>
        /// Learns a regression neural network
        /// </summary>
        /// <param name="observations"></param>
        /// <param name="targets"></param>
        /// <returns></returns>
        IPredictorModel<double> ILearner<double>.Learn(F64Matrix observations, double[] targets)
        {
            return Learn(observations, targets);
        }

        /// <summary>
        /// Learns a regression neural network
        /// </summary>
        /// <param name="observations"></param>
        /// <param name="targets"></param>
        /// <param name="indices"></param>
        /// <returns></returns>
        IPredictorModel<double> IIndexedLearner<double>.Learn(F64Matrix observations, double[] targets, int[] indices)
        {
            return Learn(observations, targets, indices);
        }
    }
}
