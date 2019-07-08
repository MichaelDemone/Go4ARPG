using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace G4AW2.Utils
{
    public static class TransformToCanvas
    {

        private static Vector2 WorldBottomLeft(RectTransform rt)
        {
            Vector3[] corners = new Vector3[4];
            rt.GetWorldCorners(corners);
            return corners[0];
        }

        private static Vector2 LocalBottomLeft(RectTransform rt)
        {
            Vector3[] corners = new Vector3[4];
            rt.GetLocalCorners(corners);
            return corners[0];
        }

        private static Vector2 WorldCenter(RectTransform rt)
        {
            Vector2 center = new Vector2(0, 0);
            Vector3[] corners = new Vector3[4];
            rt.GetWorldCorners(corners);
            center.y = (corners[1].y+corners[0].y)/2;
            center.x = (corners[2].x+corners[1].x)/2;
            return center;
        }

        private static Vector2 GetTransformScale(Vector2 worldCenter, Vector2 worldBottomLeft, Vector2 localBottomLeft)
        {
            Vector2 scale = new Vector2(0, 0);

            scale.x = localBottomLeft.x/(worldCenter.x-worldBottomLeft.x);
            scale.y = localBottomLeft.y/(worldCenter.y-worldBottomLeft.y);

            return scale;
        }

        public static Vector3 Transform(Vector3 position, RectTransform rt)
        {
            Vector3 result = new Vector3(0, 0, 0);
            Vector2 center = WorldCenter(rt);
            Vector2 scale = GetTransformScale(center, WorldBottomLeft(rt), LocalBottomLeft(rt));

            result.x = (center.x-position.x)*scale.x;
            result.y = (center.y-position.y)*scale.y;

            return result;
        }

         private static Rect BoundingRectangle(RectTransform localRt, RectTransform rt)
         {
             Vector3[] corners = new Vector3[4];
             Vector3[] rtCorners = new Vector3[4];
             rt.GetWorldCorners(rtCorners);
             for(int i = 0 ; i < rtCorners.Length ; i++)
             {
                 corners[i] = Transform(rtCorners[i], localRt);
             }

            Rect rect = new Rect();
            rect.xMin = corners[0].x;
            rect.xMax = corners[2].x;
            rect.yMin = corners[0].y;
            rect.yMax = corners[1].y;


            return rect;
         }

        public static bool isBounded( Vector3 vec, Rect rect)
         {
             if (vec.x > rect.xMin && vec.x < rect.xMax && vec.y > rect.yMin && vec.y < rect.yMax) return true;
             else return false;
         }

        public static bool isBounded(Vector3 vec, RectTransform local, RectTransform rt)
        {
            Rect rect = BoundingRectangle(local, rt);
            if (vec.x > rect.xMin && vec.x < rect.xMax && vec.y > rect.yMin && vec.y < rect.yMax) return true;
            else return false;
        }

        public static bool isBounded(Vector3 vec, RectTransform local, RectTransform rt, float offset)
        {

            Rect rect = BoundingRectangle(local, rt);
            if (vec.x > rect.xMin && vec.x < rect.xMax && vec.y > rect.yMin+offset && vec.y < rect.yMax+offset) return true;
            else return false;
        }


        public static string convert(Rect rect)
        {
            string str = "";
            str = "Xmin: " + rect.xMin + "    :    xMax: " +rect.xMax + "    :    yMin: " +rect.yMin + "    :    yMax: " +rect.yMax;
            return str;
        }


        public static Vector2 alignTopCorner(Rect alignTo, Rect toAlign)
        {
            return new Vector2(0,alignTo.yMax - toAlign.yMax);
        }


    }
}
