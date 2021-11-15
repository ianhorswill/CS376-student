using UnityEngine;

namespace MathUtil
{
    public class MathUtil
    {
        public static float Gaussian2D (float x, float y, float s) {
            return Gaussian2D(x, y, 0, 0, s, s);
        }

        public static float Gaussian2D (float x, float y, float xmean, float ymean, float sx, float sy) {
            float x_part = Mathf.Pow((x-xmean), 2) / (2 * Mathf.Pow(sx, 2));
            float y_part = Mathf.Pow((y-ymean), 2) / (2 * Mathf.Pow(sy, 2));
            float exp = -1 * (x_part + y_part);
            return Mathf.Exp(exp);
        }
    }
}

