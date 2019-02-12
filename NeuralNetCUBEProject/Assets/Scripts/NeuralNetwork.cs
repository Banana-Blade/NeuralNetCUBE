using UnityEngine;

public class NeuralNetwork : MonoBehaviour
{
    // Set values in Inspector or at runtime! These are just first guesses while implementing.
    public int inputNeurons = 7;
    public int hiddenNeurons = 9;
    public int outputNeurons = 2;

    public float randomizeMin = -1.0f;
    public float randomizeMax = 1.0f;

    public Matrix weightsInputHidden;
    public Matrix weightsHiddenOutput;
    public Matrix biasHidden;
    public Matrix biasOutput;

    private Matrix BatchDeltaWeightsInputHidden;
    private Matrix BatchDeltaWeightsHiddenOutput;
    private Matrix BatchDeltaBiasHidden;
    private Matrix BatchDeltaBiasOutput;

    [Range(0,1)]
    public float learningRate = 0.02f;
    public int epochs = 10;
    public int batchsize = 1;
    public int batchIndex = 0;
    [Range(0, 1)]
    public float difficulty = 0.5f;

    // So that there is only one NN in the scene and will never be overwritten!
    public static NeuralNetwork instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void BuildNN()
    {
        weightsInputHidden = new Matrix(inputNeurons, hiddenNeurons);
        weightsHiddenOutput = new Matrix(hiddenNeurons, outputNeurons);
        biasHidden = new Matrix(1, hiddenNeurons);
        biasOutput = new Matrix(1, outputNeurons);

        // better in (-0.5,0.5) or (-0.3,0.3)?!
        weightsInputHidden.Randomize(randomizeMin, randomizeMax);
        weightsHiddenOutput.Randomize(randomizeMin, randomizeMax);
        biasHidden.Randomize(randomizeMin, randomizeMax);
        biasOutput.Randomize(randomizeMin, randomizeMax);

        BatchDeltaWeightsInputHidden = new Matrix(inputNeurons, hiddenNeurons);
        BatchDeltaWeightsHiddenOutput = new Matrix(hiddenNeurons, outputNeurons);
        BatchDeltaBiasHidden = new Matrix(1, hiddenNeurons);
        BatchDeltaBiasOutput = new Matrix(1, outputNeurons);

        BatchDeltaWeightsInputHidden.SetToZero();
        BatchDeltaWeightsHiddenOutput.SetToZero();
        BatchDeltaBiasHidden.SetToZero();
        BatchDeltaBiasOutput.SetToZero();
    }

    public Matrix Feedforward(Matrix input)
    {
        // net with bias
        Matrix hidden = Matrix.Addition(Matrix.MatrixMultiplication(input, weightsInputHidden), biasHidden);
        // f(net)
        hidden.UseActivationFunction();

        // net with bias
        Matrix output = Matrix.Addition(Matrix.MatrixMultiplication(hidden, weightsHiddenOutput), biasOutput);
        // f(net)
        output.UseActivationFunction();

        // Threshold function
        if (output[0, 0] >= 0.5f)
        {
            output[0, 0] = 1.0f;
        } else
        {
            output[0, 0] = 0.0f;
        }

        if (output[0, 1] >= 0.5f)
        {
            output[0, 1] = 1.0f;
        }
        else
        {
            output[0, 1] = 0.0f;
        }

        return output;
    }

    public float Backpropagation(Matrix input, Matrix target)
    {
        // like feedforward above
        Matrix hidden = Matrix.Addition(Matrix.MatrixMultiplication(input, weightsInputHidden), biasHidden);
        hidden.UseActivationFunction();
        Matrix output = Matrix.Addition(Matrix.MatrixMultiplication(hidden, weightsHiddenOutput), biasOutput);
        output.UseActivationFunction();

        /*
        // Threshold function to stop when close enough (0.8 and 0.2)
        if (output[0, 0] >= 0.8f)
        {
            output[0, 0] = 1.0f;
        }
        else if (output[0, 0] <= 0.2f)
        {
            output[0, 0] = 0.0f;
        }

        if (output[0, 1] >= 0.8f)
        {
            output[0, 1] = 1.0f;
        }
        else if (output[0, 1] <= 0.2f)
        {
            output[0, 1] = 0.0f;
        }
        */
        

        // use formulas from University Regensburg Backpropagation!
        Matrix temp1 = output.Copy();
        Matrix temp2 = output.Copy();
        temp1.ScalarMultiplication(-1);
        temp1 = Matrix.Addition(target, temp1);
        temp2.UseDerivation();
        Matrix deltaOutput = Matrix.HadamardProduct(temp1, temp2);

        Matrix temp3 = hidden.Copy();
        temp3.UseDerivation();
        Matrix temp4 = Matrix.MatrixMultiplication(deltaOutput, Matrix.Transpose(weightsHiddenOutput));
        Matrix deltaHidden = Matrix.HadamardProduct(temp3, temp4);

        Matrix BigDeltaHiddenOutput = Matrix.MatrixMultiplication(Matrix.Transpose(hidden), deltaOutput);
        Matrix BigDeltaInputHidden = Matrix.MatrixMultiplication(Matrix.Transpose(input), deltaHidden);
        BigDeltaHiddenOutput.ScalarMultiplication(learningRate);
        BigDeltaInputHidden.ScalarMultiplication(learningRate);

        BatchDeltaWeightsHiddenOutput.AdditionInPlace(BigDeltaHiddenOutput);
        BatchDeltaWeightsInputHidden.AdditionInPlace(BigDeltaInputHidden);

        // bias is like neuron with constant output 1; apply the same formulas
        Matrix temp5 = deltaOutput.Copy();
        temp5.ScalarMultiplication(learningRate);
        Matrix temp6 = deltaHidden.Copy();
        temp6.ScalarMultiplication(learningRate);

        BatchDeltaBiasOutput.AdditionInPlace(temp5);
        BatchDeltaBiasHidden.AdditionInPlace(temp6);

        // return Error: sum(t-o) which is not really the Errorfunction, because weights are already updated in between! (depends on Batchsize)
        float error = 0;
        for(int i = 0; i < temp1.Cols; i++)
        {
            error += temp1[0, i] * temp1[0, i];
        }
        // input.Print();
        // target.Print();
        // Debug.Log(error);

        batchIndex++;
        if (batchIndex == batchsize)
        {
            UpdateWeightsAndBiases();
            batchIndex = 0;
        }

        return error; // should be in [0,2]
    }

    public void UpdateWeightsAndBiases()
    {
        weightsHiddenOutput.AdditionInPlace(BatchDeltaWeightsHiddenOutput);
        weightsInputHidden.AdditionInPlace(BatchDeltaWeightsInputHidden);

        biasOutput.AdditionInPlace(BatchDeltaBiasOutput);
        biasHidden.AdditionInPlace(BatchDeltaBiasHidden);

        BatchDeltaWeightsInputHidden.SetToZero();
        BatchDeltaWeightsHiddenOutput.SetToZero();
        BatchDeltaBiasHidden.SetToZero();
        BatchDeltaBiasOutput.SetToZero();
    }

    public void ResetNetwork()
    {
        weightsInputHidden.Randomize(randomizeMin, randomizeMax);
        weightsHiddenOutput.Randomize(randomizeMin, randomizeMax);
        biasHidden.Randomize(randomizeMin, randomizeMax);
        biasOutput.Randomize(randomizeMin, randomizeMax);
    }
}
