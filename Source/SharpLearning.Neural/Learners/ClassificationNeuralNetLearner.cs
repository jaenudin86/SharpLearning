﻿using SharpLearning.Common.Interfaces;
using SharpLearning.Containers;
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
    /// ClassificationNeuralNet learner using mini-batch gradient descent. 
    /// Several optimization methods is availible through the constructor.
    /// </summary>
    public sealed class ClassificationNeuralNetLearner : IIndexedLearner<double>, IIndexedLearner<ProbabilityPrediction>,
        ILearner<double>, ILearner<ProbabilityPrediction>
    {
        readonly NeuralNetLearner m_learner;

        /// <summary>
        /// ClassificationNeuralNet learner using mini-batch gradient descent. 
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
        public ClassificationNeuralNetLearner(NeuralNet net, ILoss loss, double learningRate = 0.01, int iterations = 100, int batchSize = 128, double l1decay = 0, double l2decay = 0,
            OptimizerMethod optimizerMethod = OptimizerMethod.Adagrad, double momentum = 0.9, double ro = 0.95, double beta1 = 0.9, double beta2 = 0.999)
        {
            if (!(net.Layers.Last() is IClassificationLayer))
            {
                throw new ArgumentException("Last layer must be a classification layer type. Was: " + net.Layers.Last().GetType().Name);
            }

            m_learner = new NeuralNetLearner(net, new OneOfNTargetEncoder(), loss,
                learningRate, iterations, batchSize, l1decay, l2decay, optimizerMethod, momentum, ro, beta1, beta2);
        }

        /// <summary>
        /// Learns a classification neural network
        /// </summary>
        /// <param name="observations"></param>
        /// <param name="targets"></param>
        /// <returns></returns>
        public ClassificationNeuralNetModel Learn(F64Matrix observations, double[] targets)
        {
            var targetNames = GetOrderedTargetNames(targets);
            var model = m_learner.Learn(observations, targets);
            return new ClassificationNeuralNetModel(model, targetNames);
        }

        /// <summary>
        /// Learns a classification neural network
        /// </summary>
        /// <param name="observations"></param>
        /// <param name="targets"></param>
        /// <param name="indices"></param>
        /// <returns></returns>
        public ClassificationNeuralNetModel Learn(F64Matrix observations, double[] targets, int[] indices)
        {
            var targetNames = GetOrderedTargetNames(targets);
            var model = m_learner.Learn(observations, targets, indices);
            return new ClassificationNeuralNetModel(model, targetNames);
        }

        /// <summary>
        /// Learns a classification neural network
        /// </summary>
        /// <param name="observations"></param>
        /// <param name="targets"></param>
        /// <returns></returns>
        IPredictorModel<double> ILearner<double>.Learn(F64Matrix observations, double[] targets)
        {
            return Learn(observations, targets);
        }

        /// <summary>
        /// Learns a classification neural network
        /// </summary>
        /// <param name="observations"></param>
        /// <param name="targets"></param>
        /// <param name="indices"></param>
        /// <returns></returns>
        IPredictorModel<ProbabilityPrediction> IIndexedLearner<ProbabilityPrediction>.Learn(F64Matrix observations, double[] targets, int[] indices)
        {
            return Learn(observations, targets, indices);
        }

        IPredictorModel<double> IIndexedLearner<double>.Learn(F64Matrix observations, double[] targets, int[] indices)
        {
            return Learn(observations, targets, indices);
        }

        /// <summary>
        /// Learns a classification neural network
        /// </summary>
        /// <param name="observations"></param>
        /// <param name="targets"></param>
        /// <returns></returns>
        IPredictorModel<ProbabilityPrediction> ILearner<ProbabilityPrediction>.Learn(F64Matrix observations, double[] targets)
        {
            return Learn(observations, targets);
        }

        double[] GetOrderedTargetNames(double[] targets)
        {
            return targets.Distinct().OrderBy(v => v).ToArray();
        }
    }
}
