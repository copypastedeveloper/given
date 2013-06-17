using System;

namespace Given.Common
{
    public class Levenshtein
    {
        /// *****************************
        /// Compute Levenshtein distance 
        /// Memory efficient version
        /// *****************************
        public int Distance(String sRow, String sCol)
        {
            int rowLen = sRow.Length; // length of sRow
            int colLen = sCol.Length; // length of sCol
            int rowIdx; // iterates through sRow
            int colIdx; // iterates through sCol
            int cost; // cost

            // Test string length
            if (Math.Max(sRow.Length, sCol.Length) > Math.Pow(2, 31))
                throw (new Exception("\nMaximum string length in Levenshtein.iLD is " + Math.Pow(2, 31) + ".\nYours is " +
                                     Math.Max(sRow.Length, sCol.Length) + "."));

            // Step 1

            if (rowLen == 0)
            {
                return colLen;
            }

            if (colLen == 0)
            {
                return rowLen;
            }

            // Create the two vectors
            var v0 = new int[rowLen + 1];
            var v1 = new int[rowLen + 1];
            int[] vTmp;


            // Step 2
            // Initialize the first vector
            for (rowIdx = 1; rowIdx <= rowLen; rowIdx++)
            {
                v0[rowIdx] = rowIdx;
            }

            // Step 3

            // Fore each column
            for (colIdx = 1; colIdx <= colLen; colIdx++)
            {
                // Set the 0'th element to the column number
                v1[0] = colIdx;

                char Col_j = sCol[colIdx - 1]; // jth character of sCol


                // Step 4

                // Fore each row
                for (rowIdx = 1; rowIdx <= rowLen; rowIdx++)
                {
                    char Row_i = sRow[rowIdx - 1]; // ith character of sRow


                    // Step 5

                    if (Row_i == Col_j)
                    {
                        cost = 0;
                    }
                    else
                    {
                        cost = 1;
                    }

                    // Step 6

                    // Find minimum
                    int m_min = v0[rowIdx] + 1;
                    int b = v1[rowIdx - 1] + 1;
                    int c = v0[rowIdx - 1] + cost;

                    if (b < m_min)
                    {
                        m_min = b;
                    }
                    if (c < m_min)
                    {
                        m_min = c;
                    }

                    v1[rowIdx] = m_min;
                }

                // Swap the vectors
                vTmp = v0;
                v0 = v1;
                v1 = vTmp;
            }


            // Step 7

            // Value between 0 - 100
            // 0==perfect match 100==totaly different
            // 
            // The vectors where swaped one last time at the end of the last loop,
            // that is why the result is now in v0 rather than in v1
            int max = Math.Max(rowLen, colLen);
            return ((100*v0[rowLen])/max);
        }


        // *****************************
        // Compute the min
        // *****************************
        int Minimum(int a, int b, int c)
        {
            int mi = a;

            if (b < mi)
            {
                mi = b;
            }
            if (c < mi)
            {
                mi = c;
            }

            return mi;
        }
    }
}