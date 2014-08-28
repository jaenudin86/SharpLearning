﻿using SharpLearning.GradientBoost.LossFunctions;

namespace SharpLearning.GradientBoost.Learners
{
    /// <summary>
    /// Multi-class classification gradient boosting usibg multinomial deviance loss
    /// http://statweb.stanford.edu/~jhf/ftp/trebst.pdf
    /// A series of regression trees are fitted stage wise on the residuals of the previous stage.
    /// The resulting models are ensembled together using addition.
    /// </summary>
    public sealed class MultinomialDevianceClassificationGradientBoostLearner : ClassificationGradientBoostLearner
    {
        /// <summary>
        /// Multi-class classification gradient boosting usibg multinomial deviance loss
        /// A series of regression trees are fitted stage wise on the probability residuals of the previous stage.
        /// A set of regression trees equal to the number of classes are fitted at each stage to estimate the class probabilities.
        /// </summary>
        /// <param name="iterations">The number of iterations or stages</param>
        /// <param name="learningRate">How much each iteration should contribute with</param>
        /// <param name="maximumTreeDepth">The maximum depth of the tree models</param>
        /// <param name="maximumLeafCount">The maximum leaf count of the tree models</param>
        /// <param name="minimumSplitSize">minimum node split size in the trees 1 is default</param>
        /// <param name="minimumInformationGain">The minimum improvement in information gain before a split is made</param>
        public MultinomialDevianceClassificationGradientBoostLearner(int iterations = 100, double learningRate = 0.1, int maximumTreeDepth = 3,
            int maximumLeafCount = 2000, int minimumSplitSize = 1, double minimumInformationGain = 0.000001)
            : base(new MultinomialDevianceClassificationLossFunction(learningRate), iterations, maximumTreeDepth, maximumLeafCount,
                    minimumSplitSize, minimumInformationGain)
        {
        }
    }
}
