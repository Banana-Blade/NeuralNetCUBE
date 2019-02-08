using UnityEngine;

public class NeuralNetwork : MonoBehaviour
{
    // Set values in Inspector or at runtime! These are just first guesses while implementing.
    public int inputNeurons = 11;
    public int hiddenNeurons = 9;
    public int outputNeurons = 2;

    public Matrix weightsInputHidden;
    public Matrix weightsHiddenOutput;
    public Matrix biasHidden;
    public Matrix biasOutput;

    [Range(0,1)]
    public float learningRate = 0.9f;

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

    void Start()
    {
        weightsInputHidden = new Matrix(inputNeurons, hiddenNeurons);
        weightsHiddenOutput = new Matrix(hiddenNeurons, outputNeurons);
        biasHidden = new Matrix(1, hiddenNeurons);
        biasOutput = new Matrix(1, outputNeurons);

        // better in (-0.5,0.5) or (-0.3,0.3)?!
        weightsInputHidden.Randomize(-1,1);
        weightsHiddenOutput.Randomize(-1, 1);
        biasHidden.Randomize(-1, 1);
        biasOutput.Randomize(-1, 1);
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
        
        return output;
    }

    public void Backpropagation(Matrix input, Matrix target)
    {
        // like feedforward above
        Matrix hidden = Matrix.Addition(Matrix.MatrixMultiplication(input, weightsInputHidden), biasHidden);
        hidden.UseActivationFunction();
        Matrix output = Matrix.Addition(Matrix.MatrixMultiplication(hidden, weightsHiddenOutput), biasOutput);
        output.UseActivationFunction();

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

        weightsHiddenOutput = Matrix.Addition(weightsHiddenOutput, BigDeltaHiddenOutput);
        weightsInputHidden = Matrix.Addition(weightsInputHidden, BigDeltaInputHidden);

        // bias is like neuron with constant output 1; apply the same formulas
        Matrix temp5 = deltaOutput.Copy();
        temp5.ScalarMultiplication(learningRate);
        Matrix temp6 = deltaHidden.Copy();
        temp6.ScalarMultiplication(learningRate);
        biasOutput = Matrix.Addition(biasOutput, temp5);
        biasHidden = Matrix.Addition(biasHidden, temp6);
    }
}
