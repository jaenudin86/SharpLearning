﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpLearning.Containers;
using SharpLearning.Containers.Matrices;
using SharpLearning.Metrics.Regression;
using SharpLearning.Neural.Layers;
using SharpLearning.Neural.Learners;
using SharpLearning.Neural.Loss;
using SharpLearning.Neural.Models;
using System;
using System.IO;
using System.Linq;

namespace SharpLearning.Neural.Test.Models
{
    [TestClass]
    public class RegressionNeuralNetModelTest
    {
        [TestMethod]
        public void RegressionNeuralNetModel_Predict_Single()
        {
            var numberOfObservations = 500;
            var numberOfFeatures = 5;

            var random = new Random(32);
            var observations = new F64Matrix(numberOfObservations, numberOfFeatures);
            observations.Initialize(() => random.NextDouble());
            var targets = Enumerable.Range(0, numberOfObservations).Select(i => (double)random.NextDouble()).ToArray();

            var sut = RegressionNeuralNetModel.Load(() => new StringReader(RegressionNeuralNetModelText));

            var predictions = new double[numberOfObservations];
            for (int i = 0; i < numberOfObservations; i++)
            {
                predictions[i] = sut.Predict(observations.GetRow(i));
            }

            var evaluator = new MeanSquaredErrorRegressionMetric();
            var actual = evaluator.Error(targets, predictions);

            Assert.AreEqual(0.086988999595858624, actual, 0.0001);
        }

        [TestMethod]
        public void RegressionNeuralNetModel_Predict_Multiple()
        {
            var numberOfObservations = 500;
            var numberOfFeatures = 5;

            var random = new Random(32);
            var observations = new F64Matrix(numberOfObservations, numberOfFeatures);
            observations.Initialize(() => random.NextDouble());
            var targets = Enumerable.Range(0, numberOfObservations).Select(i => (double)random.NextDouble()).ToArray();

            var sut = RegressionNeuralNetModel.Load(() => new StringReader(RegressionNeuralNetModelText));

            var predictions = sut.Predict(observations);

            var evaluator = new MeanSquaredErrorRegressionMetric();
            var actual = evaluator.Error(targets, predictions);

            Assert.AreEqual(0.086988999595858624, actual, 0.0001);
        }

        [TestMethod]
        public void RegressionNeuralNetModel_Save()
        {
            var numberOfObservations = 500;
            var numberOfFeatures = 5;

            var random = new Random(32);
            var observations = new F64Matrix(numberOfObservations, numberOfFeatures);
            observations.Initialize(() => random.NextDouble());
            var targets = Enumerable.Range(0, numberOfObservations).Select(i => (double)random.NextDouble()).ToArray();

            var net = new NeuralNet();
            net.Add(new InputLayer(numberOfFeatures));
            net.Add(new DenseLayer(10));
            net.Add(new SquaredErrorRegressionLayer());

            var learner = new RegressionNeuralNetLearner(net, new AccuracyLoss());
            var sut = learner.Learn(observations, targets);

            var writer = new StringWriter();
            sut.Save(() => writer);

            var actual = writer.ToString();
            Assert.AreEqual(RegressionNeuralNetModelText, actual);
        }

        string RegressionNeuralNetModelText = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<RegressionNeuralNetModel xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" z:Id=\"1\" xmlns:z=\"http://schemas.microsoft.com/2003/10/Serialization/\" xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Models\">\r\n  <m_neuralNet xmlns:d2p1=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural\" z:Id=\"2\">\r\n    <d2p1:Layers xmlns:d3p1=\"http://schemas.microsoft.com/2003/10/Serialization/Arrays\" z:Id=\"3\" z:Size=\"5\">\r\n      <d3p1:anyType z:Id=\"4\" xmlns:d4p1=\"SharpLearning.Neural.Layers\" i:type=\"d4p1:InputLayer\">\r\n        <_x003C_ActivationFunc_x003E_k__BackingField xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">Undefined</_x003C_ActivationFunc_x003E_k__BackingField>\r\n        <_x003C_Depth_x003E_k__BackingField xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">5</_x003C_Depth_x003E_k__BackingField>\r\n        <_x003C_Height_x003E_k__BackingField xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">1</_x003C_Height_x003E_k__BackingField>\r\n        <_x003C_Width_x003E_k__BackingField xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">1</_x003C_Width_x003E_k__BackingField>\r\n      </d3p1:anyType>\r\n      <d3p1:anyType z:Id=\"5\" xmlns:d4p1=\"SharpLearning.Neural.Layers\" i:type=\"d4p1:DenseLayer\">\r\n        <Bias xmlns:d5p1=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra\" z:Id=\"6\" xmlns:d5p2=\"MathNet.Numerics.LinearAlgebra.Single\" i:type=\"d5p2:DenseVector\" xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">\r\n          <d5p1:_x003C_Count_x003E_k__BackingField>10</d5p1:_x003C_Count_x003E_k__BackingField>\r\n          <d5p1:_x003C_Storage_x003E_k__BackingField xmlns:d6p1=\"urn:MathNet/Numerics/LinearAlgebra\" z:Id=\"7\" i:type=\"d6p1:DenseVectorStorageOffloat\">\r\n            <d6p1:Length>10</d6p1:Length>\r\n            <d6p1:Data z:Id=\"8\" z:Size=\"10\">\r\n              <d3p1:float>0.08061107</d3p1:float>\r\n              <d3p1:float>-0.0908182859</d3p1:float>\r\n              <d3p1:float>-0.0482303835</d3p1:float>\r\n              <d3p1:float>0.0260934122</d3p1:float>\r\n              <d3p1:float>-0.101279378</d3p1:float>\r\n              <d3p1:float>0.14512825</d3p1:float>\r\n              <d3p1:float>-0.07551032</d3p1:float>\r\n              <d3p1:float>-0.0408956259</d3p1:float>\r\n              <d3p1:float>-0.0619095564</d3p1:float>\r\n              <d3p1:float>0.1641399</d3p1:float>\r\n            </d6p1:Data>\r\n          </d5p1:_x003C_Storage_x003E_k__BackingField>\r\n          <_length xmlns=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra.Single\">10</_length>\r\n          <_values z:Ref=\"8\" i:nil=\"true\" xmlns=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra.Single\" />\r\n        </Bias>\r\n        <BiasGradients xmlns:d5p1=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra\" i:nil=\"true\" xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\" />\r\n        <OutputActivations xmlns:d5p1=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra\" z:Id=\"9\" xmlns:d5p2=\"MathNet.Numerics.LinearAlgebra.Single\" i:type=\"d5p2:DenseMatrix\" xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">\r\n          <d5p1:_x003C_ColumnCount_x003E_k__BackingField>10</d5p1:_x003C_ColumnCount_x003E_k__BackingField>\r\n          <d5p1:_x003C_RowCount_x003E_k__BackingField>1</d5p1:_x003C_RowCount_x003E_k__BackingField>\r\n          <d5p1:_x003C_Storage_x003E_k__BackingField xmlns:d6p1=\"urn:MathNet/Numerics/LinearAlgebra\" z:Id=\"10\" i:type=\"d6p1:DenseColumnMajorMatrixStorageOffloat\">\r\n            <d6p1:RowCount>1</d6p1:RowCount>\r\n            <d6p1:ColumnCount>10</d6p1:ColumnCount>\r\n            <d6p1:Data z:Id=\"11\" z:Size=\"10\">\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n            </d6p1:Data>\r\n          </d5p1:_x003C_Storage_x003E_k__BackingField>\r\n          <_columnCount xmlns=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra.Single\">10</_columnCount>\r\n          <_rowCount xmlns=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra.Single\">1</_rowCount>\r\n          <_values z:Ref=\"11\" i:nil=\"true\" xmlns=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra.Single\" />\r\n        </OutputActivations>\r\n        <Weights xmlns:d5p1=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra\" z:Id=\"12\" xmlns:d5p2=\"MathNet.Numerics.LinearAlgebra.Single\" i:type=\"d5p2:DenseMatrix\" xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">\r\n          <d5p1:_x003C_ColumnCount_x003E_k__BackingField>10</d5p1:_x003C_ColumnCount_x003E_k__BackingField>\r\n          <d5p1:_x003C_RowCount_x003E_k__BackingField>5</d5p1:_x003C_RowCount_x003E_k__BackingField>\r\n          <d5p1:_x003C_Storage_x003E_k__BackingField xmlns:d6p1=\"urn:MathNet/Numerics/LinearAlgebra\" z:Id=\"13\" i:type=\"d6p1:DenseColumnMajorMatrixStorageOffloat\">\r\n            <d6p1:RowCount>5</d6p1:RowCount>\r\n            <d6p1:ColumnCount>10</d6p1:ColumnCount>\r\n            <d6p1:Data z:Id=\"14\" z:Size=\"50\">\r\n              <d3p1:float>0.122887827</d3p1:float>\r\n              <d3p1:float>0.315400332</d3p1:float>\r\n              <d3p1:float>0.0469409</d3p1:float>\r\n              <d3p1:float>0.111320414</d3p1:float>\r\n              <d3p1:float>0.483705729</d3p1:float>\r\n              <d3p1:float>-0.190726787</d3p1:float>\r\n              <d3p1:float>0.0684103742</d3p1:float>\r\n              <d3p1:float>-0.4401247</d3p1:float>\r\n              <d3p1:float>0.2862351</d3p1:float>\r\n              <d3p1:float>-0.218659639</d3p1:float>\r\n              <d3p1:float>-0.03549691</d3p1:float>\r\n              <d3p1:float>-0.463935018</d3p1:float>\r\n              <d3p1:float>0.231475249</d3p1:float>\r\n              <d3p1:float>0.105058372</d3p1:float>\r\n              <d3p1:float>0.148711</d3p1:float>\r\n              <d3p1:float>-0.139096379</d3p1:float>\r\n              <d3p1:float>0.07078468</d3p1:float>\r\n              <d3p1:float>-0.237818852</d3p1:float>\r\n              <d3p1:float>0.243174955</d3p1:float>\r\n              <d3p1:float>0.167189375</d3p1:float>\r\n              <d3p1:float>0.158287868</d3p1:float>\r\n              <d3p1:float>-0.178924575</d3p1:float>\r\n              <d3p1:float>0.279462725</d3p1:float>\r\n              <d3p1:float>-0.1277468</d3p1:float>\r\n              <d3p1:float>-0.272537261</d3p1:float>\r\n              <d3p1:float>-0.000809151854</d3p1:float>\r\n              <d3p1:float>0.1048457</d3p1:float>\r\n              <d3p1:float>0.2587103</d3p1:float>\r\n              <d3p1:float>-0.0537242629</d3p1:float>\r\n              <d3p1:float>-0.1936619</d3p1:float>\r\n              <d3p1:float>-0.09270535</d3p1:float>\r\n              <d3p1:float>0.110871442</d3p1:float>\r\n              <d3p1:float>0.231537</d3p1:float>\r\n              <d3p1:float>0.214023471</d3p1:float>\r\n              <d3p1:float>0.295986354</d3p1:float>\r\n              <d3p1:float>0.121263891</d3p1:float>\r\n              <d3p1:float>-0.226977482</d3p1:float>\r\n              <d3p1:float>-0.166032359</d3p1:float>\r\n              <d3p1:float>-0.407895952</d3p1:float>\r\n              <d3p1:float>0.0514866374</d3p1:float>\r\n              <d3p1:float>0.372274041</d3p1:float>\r\n              <d3p1:float>-0.0744693056</d3p1:float>\r\n              <d3p1:float>-0.420041</d3p1:float>\r\n              <d3p1:float>-0.1777532</d3p1:float>\r\n              <d3p1:float>-0.207031131</d3p1:float>\r\n              <d3p1:float>-0.08029695</d3p1:float>\r\n              <d3p1:float>-0.291836649</d3p1:float>\r\n              <d3p1:float>0.6121856</d3p1:float>\r\n              <d3p1:float>0.1480729</d3p1:float>\r\n              <d3p1:float>-0.0147523507</d3p1:float>\r\n            </d6p1:Data>\r\n          </d5p1:_x003C_Storage_x003E_k__BackingField>\r\n          <_columnCount xmlns=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra.Single\">10</_columnCount>\r\n          <_rowCount xmlns=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra.Single\">5</_rowCount>\r\n          <_values z:Ref=\"14\" i:nil=\"true\" xmlns=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra.Single\" />\r\n        </Weights>\r\n        <WeightsGradients xmlns:d5p1=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra\" i:nil=\"true\" xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\" />\r\n        <_x003C_ActivationFunc_x003E_k__BackingField xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">Relu</_x003C_ActivationFunc_x003E_k__BackingField>\r\n        <_x003C_Depth_x003E_k__BackingField xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">10</_x003C_Depth_x003E_k__BackingField>\r\n        <_x003C_Height_x003E_k__BackingField xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">1</_x003C_Height_x003E_k__BackingField>\r\n        <_x003C_UseBatchNormalization_x003E_k__BackingField xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">false</_x003C_UseBatchNormalization_x003E_k__BackingField>\r\n        <_x003C_Width_x003E_k__BackingField xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">1</_x003C_Width_x003E_k__BackingField>\r\n        <m_delta xmlns:d5p1=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra\" i:nil=\"true\" xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\" />\r\n        <m_inputActivations xmlns:d5p1=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra\" i:nil=\"true\" xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\" />\r\n      </d3p1:anyType>\r\n      <d3p1:anyType z:Id=\"15\" xmlns:d4p1=\"SharpLearning.Neural.Layers\" i:type=\"d4p1:ActivationLayer\">\r\n        <ActivationDerivative xmlns:d5p1=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra\" z:Id=\"16\" xmlns:d5p2=\"MathNet.Numerics.LinearAlgebra.Single\" i:type=\"d5p2:DenseMatrix\" xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">\r\n          <d5p1:_x003C_ColumnCount_x003E_k__BackingField>10</d5p1:_x003C_ColumnCount_x003E_k__BackingField>\r\n          <d5p1:_x003C_RowCount_x003E_k__BackingField>1</d5p1:_x003C_RowCount_x003E_k__BackingField>\r\n          <d5p1:_x003C_Storage_x003E_k__BackingField xmlns:d6p1=\"urn:MathNet/Numerics/LinearAlgebra\" z:Id=\"17\" i:type=\"d6p1:DenseColumnMajorMatrixStorageOffloat\">\r\n            <d6p1:RowCount>1</d6p1:RowCount>\r\n            <d6p1:ColumnCount>10</d6p1:ColumnCount>\r\n            <d6p1:Data z:Id=\"18\" z:Size=\"10\">\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n            </d6p1:Data>\r\n          </d5p1:_x003C_Storage_x003E_k__BackingField>\r\n          <_columnCount xmlns=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra.Single\">10</_columnCount>\r\n          <_rowCount xmlns=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra.Single\">1</_rowCount>\r\n          <_values z:Ref=\"18\" i:nil=\"true\" xmlns=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra.Single\" />\r\n        </ActivationDerivative>\r\n        <OutputActivations xmlns:d5p1=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra\" z:Id=\"19\" xmlns:d5p2=\"MathNet.Numerics.LinearAlgebra.Single\" i:type=\"d5p2:DenseMatrix\" xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">\r\n          <d5p1:_x003C_ColumnCount_x003E_k__BackingField>10</d5p1:_x003C_ColumnCount_x003E_k__BackingField>\r\n          <d5p1:_x003C_RowCount_x003E_k__BackingField>1</d5p1:_x003C_RowCount_x003E_k__BackingField>\r\n          <d5p1:_x003C_Storage_x003E_k__BackingField xmlns:d6p1=\"urn:MathNet/Numerics/LinearAlgebra\" z:Id=\"20\" i:type=\"d6p1:DenseColumnMajorMatrixStorageOffloat\">\r\n            <d6p1:RowCount>1</d6p1:RowCount>\r\n            <d6p1:ColumnCount>10</d6p1:ColumnCount>\r\n            <d6p1:Data z:Id=\"21\" z:Size=\"10\">\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n            </d6p1:Data>\r\n          </d5p1:_x003C_Storage_x003E_k__BackingField>\r\n          <_columnCount xmlns=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra.Single\">10</_columnCount>\r\n          <_rowCount xmlns=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra.Single\">1</_rowCount>\r\n          <_values z:Ref=\"21\" i:nil=\"true\" xmlns=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra.Single\" />\r\n        </OutputActivations>\r\n        <_x003C_ActivationFunc_x003E_k__BackingField xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">Relu</_x003C_ActivationFunc_x003E_k__BackingField>\r\n        <_x003C_Depth_x003E_k__BackingField xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">10</_x003C_Depth_x003E_k__BackingField>\r\n        <_x003C_Height_x003E_k__BackingField xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">1</_x003C_Height_x003E_k__BackingField>\r\n        <_x003C_Width_x003E_k__BackingField xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">1</_x003C_Width_x003E_k__BackingField>\r\n        <m_activation z:Id=\"22\" xmlns:d5p1=\"SharpLearning.Neural.Activations\" i:type=\"d5p1:ReluActivation\" xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\" />\r\n        <m_delta xmlns:d5p1=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra\" i:nil=\"true\" xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\" />\r\n        <m_inputActivations xmlns:d5p1=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra\" i:nil=\"true\" xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\" />\r\n      </d3p1:anyType>\r\n      <d3p1:anyType z:Id=\"23\" xmlns:d4p1=\"SharpLearning.Neural.Layers\" i:type=\"d4p1:DenseLayer\">\r\n        <Bias xmlns:d5p1=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra\" z:Id=\"24\" xmlns:d5p2=\"MathNet.Numerics.LinearAlgebra.Single\" i:type=\"d5p2:DenseVector\" xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">\r\n          <d5p1:_x003C_Count_x003E_k__BackingField>1</d5p1:_x003C_Count_x003E_k__BackingField>\r\n          <d5p1:_x003C_Storage_x003E_k__BackingField xmlns:d6p1=\"urn:MathNet/Numerics/LinearAlgebra\" z:Id=\"25\" i:type=\"d6p1:DenseVectorStorageOffloat\">\r\n            <d6p1:Length>1</d6p1:Length>\r\n            <d6p1:Data z:Id=\"26\" z:Size=\"1\">\r\n              <d3p1:float>0.07907934</d3p1:float>\r\n            </d6p1:Data>\r\n          </d5p1:_x003C_Storage_x003E_k__BackingField>\r\n          <_length xmlns=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra.Single\">1</_length>\r\n          <_values z:Ref=\"26\" i:nil=\"true\" xmlns=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra.Single\" />\r\n        </Bias>\r\n        <BiasGradients xmlns:d5p1=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra\" i:nil=\"true\" xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\" />\r\n        <OutputActivations xmlns:d5p1=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra\" z:Id=\"27\" xmlns:d5p2=\"MathNet.Numerics.LinearAlgebra.Single\" i:type=\"d5p2:DenseMatrix\" xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">\r\n          <d5p1:_x003C_ColumnCount_x003E_k__BackingField>1</d5p1:_x003C_ColumnCount_x003E_k__BackingField>\r\n          <d5p1:_x003C_RowCount_x003E_k__BackingField>1</d5p1:_x003C_RowCount_x003E_k__BackingField>\r\n          <d5p1:_x003C_Storage_x003E_k__BackingField xmlns:d6p1=\"urn:MathNet/Numerics/LinearAlgebra\" z:Id=\"28\" i:type=\"d6p1:DenseColumnMajorMatrixStorageOffloat\">\r\n            <d6p1:RowCount>1</d6p1:RowCount>\r\n            <d6p1:ColumnCount>1</d6p1:ColumnCount>\r\n            <d6p1:Data z:Id=\"29\" z:Size=\"1\">\r\n              <d3p1:float>0</d3p1:float>\r\n            </d6p1:Data>\r\n          </d5p1:_x003C_Storage_x003E_k__BackingField>\r\n          <_columnCount xmlns=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra.Single\">1</_columnCount>\r\n          <_rowCount xmlns=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra.Single\">1</_rowCount>\r\n          <_values z:Ref=\"29\" i:nil=\"true\" xmlns=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra.Single\" />\r\n        </OutputActivations>\r\n        <Weights xmlns:d5p1=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra\" z:Id=\"30\" xmlns:d5p2=\"MathNet.Numerics.LinearAlgebra.Single\" i:type=\"d5p2:DenseMatrix\" xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">\r\n          <d5p1:_x003C_ColumnCount_x003E_k__BackingField>1</d5p1:_x003C_ColumnCount_x003E_k__BackingField>\r\n          <d5p1:_x003C_RowCount_x003E_k__BackingField>10</d5p1:_x003C_RowCount_x003E_k__BackingField>\r\n          <d5p1:_x003C_Storage_x003E_k__BackingField xmlns:d6p1=\"urn:MathNet/Numerics/LinearAlgebra\" z:Id=\"31\" i:type=\"d6p1:DenseColumnMajorMatrixStorageOffloat\">\r\n            <d6p1:RowCount>10</d6p1:RowCount>\r\n            <d6p1:ColumnCount>1</d6p1:ColumnCount>\r\n            <d6p1:Data z:Id=\"32\" z:Size=\"10\">\r\n              <d3p1:float>0.6289029</d3p1:float>\r\n              <d3p1:float>0.9063734</d3p1:float>\r\n              <d3p1:float>0.0207913313</d3p1:float>\r\n              <d3p1:float>1.13584745</d3p1:float>\r\n              <d3p1:float>-0.424335748</d3p1:float>\r\n              <d3p1:float>0.534019232</d3p1:float>\r\n              <d3p1:float>-1.08953667</d3p1:float>\r\n              <d3p1:float>-0.6154578</d3p1:float>\r\n              <d3p1:float>0.975284636</d3p1:float>\r\n              <d3p1:float>0.3575652</d3p1:float>\r\n            </d6p1:Data>\r\n          </d5p1:_x003C_Storage_x003E_k__BackingField>\r\n          <_columnCount xmlns=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra.Single\">1</_columnCount>\r\n          <_rowCount xmlns=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra.Single\">10</_rowCount>\r\n          <_values z:Ref=\"32\" i:nil=\"true\" xmlns=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra.Single\" />\r\n        </Weights>\r\n        <WeightsGradients xmlns:d5p1=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra\" i:nil=\"true\" xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\" />\r\n        <_x003C_ActivationFunc_x003E_k__BackingField xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">Undefined</_x003C_ActivationFunc_x003E_k__BackingField>\r\n        <_x003C_Depth_x003E_k__BackingField xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">1</_x003C_Depth_x003E_k__BackingField>\r\n        <_x003C_Height_x003E_k__BackingField xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">1</_x003C_Height_x003E_k__BackingField>\r\n        <_x003C_UseBatchNormalization_x003E_k__BackingField xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">false</_x003C_UseBatchNormalization_x003E_k__BackingField>\r\n        <_x003C_Width_x003E_k__BackingField xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">1</_x003C_Width_x003E_k__BackingField>\r\n        <m_delta xmlns:d5p1=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra\" i:nil=\"true\" xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\" />\r\n        <m_inputActivations xmlns:d5p1=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra\" i:nil=\"true\" xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\" />\r\n      </d3p1:anyType>\r\n      <d3p1:anyType z:Id=\"33\" xmlns:d4p1=\"SharpLearning.Neural.Layers\" i:type=\"d4p1:SquaredErrorRegressionLayer\">\r\n        <NumberOfTargets xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">1</NumberOfTargets>\r\n        <OutputActivations xmlns:d5p1=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra\" z:Id=\"34\" xmlns:d5p2=\"MathNet.Numerics.LinearAlgebra.Single\" i:type=\"d5p2:DenseMatrix\" xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">\r\n          <d5p1:_x003C_ColumnCount_x003E_k__BackingField>1</d5p1:_x003C_ColumnCount_x003E_k__BackingField>\r\n          <d5p1:_x003C_RowCount_x003E_k__BackingField>1</d5p1:_x003C_RowCount_x003E_k__BackingField>\r\n          <d5p1:_x003C_Storage_x003E_k__BackingField xmlns:d6p1=\"urn:MathNet/Numerics/LinearAlgebra\" z:Id=\"35\" i:type=\"d6p1:DenseColumnMajorMatrixStorageOffloat\">\r\n            <d6p1:RowCount>1</d6p1:RowCount>\r\n            <d6p1:ColumnCount>1</d6p1:ColumnCount>\r\n            <d6p1:Data z:Id=\"36\" z:Size=\"1\">\r\n              <d3p1:float>0</d3p1:float>\r\n            </d6p1:Data>\r\n          </d5p1:_x003C_Storage_x003E_k__BackingField>\r\n          <_columnCount xmlns=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra.Single\">1</_columnCount>\r\n          <_rowCount xmlns=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra.Single\">1</_rowCount>\r\n          <_values z:Ref=\"36\" i:nil=\"true\" xmlns=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra.Single\" />\r\n        </OutputActivations>\r\n        <_x003C_ActivationFunc_x003E_k__BackingField xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">Undefined</_x003C_ActivationFunc_x003E_k__BackingField>\r\n        <_x003C_Depth_x003E_k__BackingField xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">1</_x003C_Depth_x003E_k__BackingField>\r\n        <_x003C_Height_x003E_k__BackingField xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">1</_x003C_Height_x003E_k__BackingField>\r\n        <_x003C_Width_x003E_k__BackingField xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">1</_x003C_Width_x003E_k__BackingField>\r\n        <m_delta xmlns:d5p1=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra\" i:nil=\"true\" xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\" />\r\n      </d3p1:anyType>\r\n    </d2p1:Layers>\r\n  </m_neuralNet>\r\n</RegressionNeuralNetModel>";
    }
}
