using UnityEngine;

public class Matrix
{
    private readonly float[,] _matrix;

    public Matrix(int dim1, int dim2)
    {
        _matrix = new float[dim1, dim2];
    }

    public int Rows { get { return _matrix.GetLength(0); } }
    public int Cols { get { return _matrix.GetLength(1); } }

    public float this[int x, int y]
    {
        get { return _matrix[x, y]; }
        set { _matrix[x, y] = value; }
    }

    public void Randomize(float min, float max)
    {
        for (int i = 0; i < this.Rows; i++)
        {
            for (int j = 0; j < this.Cols; j++)
            {
                _matrix[i, j] = Random.Range(min, max);
            }
        }
    }

    public void SetToZero()
    {
        for (int i = 0; i < this.Rows; i++)
        {
            for (int j = 0; j < this.Cols; j++)
            {
                _matrix[i, j] = 0f;
            }
        }
    }

    public static Matrix Transpose(Matrix m)
    {
        Matrix resultMatrix = new Matrix(m.Cols, m.Rows);
        for (int i = 0; i < resultMatrix.Rows; i++)
        {
            for (int j = 0; j < resultMatrix.Cols; j++)
            {
                resultMatrix[i, j] = m[j, i];
            }
        }
        return resultMatrix;
    }

    public static Matrix MatrixMultiplication(Matrix m1, Matrix m2)
    {
        if (m1.Cols != m2.Rows)
        {
            Debug.LogError("Matrixmultiplication " + m1 + " times " + m2 + " not possible!");
            return null;
        }

        Matrix resultMatrix = new Matrix(m1.Rows, m2.Cols);
        for (int i = 0; i < resultMatrix.Rows; i++)
        {
            for (int j = 0; j < resultMatrix.Cols; j++)
            {
                resultMatrix[i, j] = 0;
                for (int k = 0; k < m1.Cols; k++)
                {
                    resultMatrix[i, j] += m1[i, k] * m2[k, j];
                }
            }
        }
        return resultMatrix;
    }

    public static Matrix Addition(Matrix m1, Matrix m2)
    {
        if ((m1.Cols != m2.Cols) || (m1.Rows != m2.Rows))
        {
            Debug.LogError("Matrixaddition " + m1 + " plus " + m2 + " not possible!");
            return null;
        }

        Matrix resultMatrix = new Matrix(m1.Rows, m1.Cols);
        for (int i = 0; i < resultMatrix.Rows; i++)
        {
            for (int j = 0; j < resultMatrix.Cols; j++)
            {
                resultMatrix[i, j] =  m1[i, j] + m2[i, j];
            }
        }
        return resultMatrix;
    }

    public void AdditionInPlace(Matrix other)
    {
        if ((this.Cols != other.Cols) || (this.Rows != other.Rows))
        {
            Debug.LogError("Matrixaddition " + this + " plus " + other + " not possible!");
            return;
        }

        for (int i = 0; i < this.Rows; i++)
        {
            for (int j = 0; j < this.Cols; j++)
            {
                _matrix[i, j] += other[i, j];
            }
        }
        return;
    }

    public void ScalarMultiplication(float factor)
    {
        for (int i = 0; i < this.Rows; i++)
        {
            for (int j = 0; j < this.Cols; j++)
            {
                _matrix[i, j] *= factor;
            }
        }
    }

    public static Matrix HadamardProduct(Matrix m1, Matrix m2)
    {
        if ((m1.Cols != m2.Cols) || (m1.Rows != m2.Rows))
        {
            Debug.LogError("Hadamard-Product " + m1 + " times " + m2 + " not possible!");
            return null;
        }

        Matrix resultMatrix = new Matrix(m1.Rows, m1.Cols);
        for (int i = 0; i < resultMatrix.Rows; i++)
        {
            for (int j = 0; j < resultMatrix.Cols; j++)
            {
                resultMatrix[i, j] = m1[i, j] * m2[i, j];
            }
        }
        return resultMatrix;
    }

    public void Print()
    {
        string s = "Matrix:\n";
        for (int i = 0; i < this.Rows; i++)
        {
            for (int j = 0; j < this.Cols; j++)
            {
                s += _matrix[i, j].ToString() + " ";
            }
            s += "\n";
        }
        Debug.LogFormat(s);
    }

    public void UseActivationFunction()
    {
        for (int i = 0; i < this.Rows; i++)
        {
            for (int j = 0; j < this.Cols; j++)
            {
                // Change this for more ActivationFunctions, maybe with parameter/Enum for this method
                _matrix[i, j] = 1.0f / (1.0f + Mathf.Exp(-_matrix[i, j]));
            }
        }
    }

    public void UseDerivation()
    {
        for (int i = 0; i < this.Rows; i++)
        {
            for (int j = 0; j < this.Cols; j++)
            {
                // Change this for more ActivationFunctions, maybe with parameter/Enum for this method
                _matrix[i, j] = _matrix[i, j] * (1.0f - _matrix[i, j]);
            }
        }
    }

    public Matrix Copy()
    {
        Matrix resultMatrix = new Matrix(this.Rows, this.Cols);
        for (int i = 0; i < resultMatrix.Rows; i++)
        {
            for (int j = 0; j < resultMatrix.Cols; j++)
            {
                resultMatrix[i, j] = _matrix[i, j];
            }
        }
        return resultMatrix;
    }
}