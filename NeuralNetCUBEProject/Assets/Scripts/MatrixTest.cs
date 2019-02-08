using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Matrix m = new Matrix(2, 3);
        m.Print();

        m[1, 2] = 7;
        m.Print();

        m.Randomize(-1,1);
        m.Print();

        Matrix n = Matrix.Transpose(m);
        n.Print();

        Matrix n2 = new Matrix(2, 3);
        Matrix m2 = new Matrix(3, 1);
        n2[0, 0] = 1;
        n2[0, 1] = 2;
        n2[0, 2] = 3;
        n2[1, 0] = 4;
        n2[1, 1] = 5;
        n2[1, 2] = 6;
        m2[0, 0] = 7;
        m2[1, 0] = 8;
        m2[2, 0] = 9;
        n2.Print();
        m2.Print();
        Matrix o = Matrix.MatrixMultiplication(n2, m2);
        o.Print();

        Matrix n3 = new Matrix(2, 3);
        n3[0, 0] = 1;
        n3[0, 1] = 0;
        n3[0, 2] = 2;
        n3[1, 0] = 2;
        n3[1, 1] = 1;
        n3[1, 2] = -1;
        n3.Print();
        // Matrix o2 = Matrix.Addition(n2, n3);
        Matrix o2 = Matrix.HadamardProduct(n2, n3);
        o2.Print();

        o2.ScalarMultiplication(2);
        o2.Print();

        // o2.UseActivationFunction();
        o2.UseDerivation();
        o2.Print();
    }
}
