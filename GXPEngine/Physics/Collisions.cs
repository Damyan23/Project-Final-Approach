using GXPEngine;
using GXPEngine.Core;

public static class Collisions
{
    // Used if the center of the polygons are passed
    public static bool IntersectPolygons(Vector2 centerA, Vector2[] verticesA, Vector2 centerB, Vector2[] verticesB, out Vector2 normal, out float depth)
    {
        // Initialize the normal vector and depth to default values
        normal = new Vector2();
        depth = float.MaxValue;

        // Loop through each vertex of polygon A
        for (int i = 0; i < verticesA.Length; i++)
        {
            // Get the current vertex and the next vertex to form an edge
            Vector2 vectorA = verticesA[i];
            Vector2 vectorB = verticesA[(i + 1) % verticesA.Length];

            // Compute the edge vector from vertex A to vertex B
            Vector2 edge = vectorB - vectorA;

            // Compute the axis perpendicular to the edge (normal of each edge)
            Vector2 axis = new Vector2(-edge.y, edge.x);
            axis.Normalize();

            // Project vertices of both polygons onto this axis and get the minimum and maximum projections
            Collisions.ProjectVertices(verticesA, axis, out float minA, out float maxA);
            Collisions.ProjectVertices(verticesB, axis, out float minB, out float maxB);

            // Check for overlap between the projections
            if (minA >= maxB || minB >= maxA)
            {
                // If there is no overlap, polygons are separated along this axis, return false
                return false;
            }

            // Compute the penetration depth along this axis
            float axisDepth = Mathf.Min(maxB - minA, maxA - minB);

            // If this is the smallest penetration depth so far, update normal and depth
            if (axisDepth < depth)
            {
                depth = axisDepth;
                normal = axis;
            }
        }

        // Repeat the same process for each edge of polygon B
        for (int i = 0; i < verticesB.Length; i++)
        {
            // Get the current vertex and the next vertex to form an edge for polygon B
            Vector2 vectorA = verticesB[i];
            Vector2 vectorB = verticesB[(i + 1) % verticesB.Length];

            // Compute the edge vector from vertex A to vertex B
            Vector2 edge = vectorB - vectorA;

            // Compute the axis perpendicular to the edge (normal of each edge)
            Vector2 axis = new Vector2(-edge.y, edge.x);
            axis.Normalize();

            // Project vertices of both polygons onto this axis and get the minimum and maximum projections
            Collisions.ProjectVertices(verticesA, axis, out float minA, out float maxA);
            Collisions.ProjectVertices(verticesB, axis, out float minB, out float maxB);

            // Check for overlap between the projections
            if (minA >= maxB || minB >= maxA)
            {
                // If there is no overlap, polygons are separated along this axis, return false
                return false;
            }

            // Compute the penetration depth along this axis
            float axisDepth = Mathf.Min(maxB - minA, maxA - minB);

            // If this is the smallest penetration depth so far, update normal and depth
            if (axisDepth < depth)
            {
                depth = axisDepth;
                normal = axis;
            }
        }

        // Compute the direction vector from centerA to centerB
        Vector2 direction = centerB - centerA;

        // Check if the direction vector aligns with the normal vector
        // If not, flip the normal vector
        if (Mathf.Dot(direction, normal) < 0f)
        {
            normal = -normal;
        }

        // If no separation along any axis, polygons intersect, return true
        return true;
    }


    // Used if the centers of the polygons are not passed
    public static bool IntersectPolygons(Vector2[] verticesA, Vector2[] verticesB, out Vector2 normal, out float depth)
    {
        normal = new Vector2();
        // Put max in since we wanna find the lowest value to resolve the collision and so every value put in here will be always lower then the max one
        depth = float.MaxValue;

        for (int i = 0; i < verticesA.Length; i++)
        {
            Vector2 vectorA = verticesA[i];
            Vector2 vectorB = verticesA[(i + 1) % verticesA.Length];

            Vector2 edge = vectorB - vectorA;
            // The axis used to check for separation, the normal of each edge
            Vector2 axis = new Vector2(-edge.y, edge.x);
            axis.Normalize();

            Collisions.ProjectVertices(verticesA, axis, out float minA, out float maxA);
            Collisions.ProjectVertices(verticesB, axis, out float minB, out float maxB);

            if (minA >= maxB || minB >= maxA)
            {
                return false;
            }

            float axisDepth = Mathf.Min(maxB - minA, maxA - minB);

            if (axisDepth < depth)
            {
                depth = axisDepth;
                normal = axis;
            }
        }

        for (int i = 0; i < verticesB.Length; i++)
        {
            Vector2 vectorA = verticesB[i];
            Vector2 vectorB = verticesB[(i + 1) % verticesB.Length];


            Vector2 edge = vectorB - vectorA;
            // The axis used to check for separation, the normal of each edge, the axis we are projecting onto
            Vector2 axis = new Vector2(-edge.y, edge.x);
            axis.Normalize();

            Collisions.ProjectVertices(verticesA, axis, out float minA, out float maxA);
            Collisions.ProjectVertices(verticesB, axis, out float minB, out float maxB);

            if (minA >= maxB || minB >= maxA)
            {
                return false;
            }

            float axisDepth = Mathf.Min(maxB - minA, maxA - minB);

            if (axisDepth < depth)
            {
                depth = axisDepth;
                normal = axis;
            }
        }

        Vector2 centerA = Collisions.FindArithmeticCenter(verticesA);
        Vector2 centerB = Collisions.FindArithmeticCenter(verticesB);

        Vector2 direction = centerB - centerA;

        if (Mathf.Dot(direction, normal) < 0f)
        {
            normal = -normal;
        }

        return true;
    }

    private static Vector2 FindArithmeticCenter(Vector2[] vertices)
    {
        // Initialize sums for x and y coordinates
        float sumX = 0f;
        float sumY = 0f;

        // Loop through each vertex of the polygon
        for (int i = 0; i < vertices.Length; i++)
        {
            // Get the current vertex
            Vector2 vector = vertices[i];

            // Accumulate x and y coordinates
            sumX += vector.x;
            sumY += vector.y;
        }

        // Calculate the average (arithmetic mean) of x and y coordinates
        float averageX = sumX / (float)vertices.Length;
        float averageY = sumY / (float)vertices.Length;

        // Return the vector representing the arithmetic center
        return new Vector2(averageX, averageY);
    }


    private static void ProjectVertices(Vector2[] vertices, Vector2 axis, out float min, out float max)
    {
        // Initialize min and max projection values
        min = float.MaxValue;
        max = float.MinValue;

        // Loop through each vertex of the polygon
        for (int i = 0; i < vertices.Length; i++)
        {
            // Get the current vertex
            Vector2 v = vertices[i];

            // Project the vertex onto the separation axis
            float projection = Mathf.Dot(v, axis);

            // Update min and max projection values
            if (projection < min) { min = projection; } // Update min if the projection is smaller
            if (projection > max) { max = projection; } // Update max if the projection is larger
        }
    }


    public static bool IntersectCircles(Vector2 centerA, float radiusA, Vector2 centerB, float radiusB, out Vector2 normal, out float depth)
    {
        // Initialize normal vector and depth
        normal = new Vector2();
        depth = 0;

        // Calculate distance between the centers of the circles
        float distance = Mathf.Distance(centerA, centerB);

        // Calculate the combined radius of both circles
        float radiusOfBothCircles = radiusA + radiusB;

        // Check if the distance between the centers is greater than or equal to the sum of the radii
        if (distance >= radiusOfBothCircles)
        {
            // If the circles are not overlapping, return false
            return false;
        }

        // Calculate the normal vector pointing from circle A to circle B
        normal = centerB - centerA;
        normal.Normalize();

        // Calculate the penetration depth (overlap) between the circles
        depth = radiusOfBothCircles - distance;

        // Circles are intersecting, return true
        return true;
    }


    // Used if the center of the polygon is passed
    public static bool IntersectCirclePolygon(Vector2 circleCenter, float circleRadius, Vector2 polygonCenter, Vector2[] vertices, out Vector2 normal, out float depth)
    {
        

        // Initialize the normal vector and depth value
        normal = new Vector2();
        depth = float.MaxValue;

        // Temporary variables to store axis, axis depth, and projection values
        Vector2 axis = new Vector2();
        float axisDepth = 0;
        float minA, maxA, minB, maxB = 0;

        // Loop through each edge of the polygon
        for (int i = 0; i < vertices.Length; i++)
        {
            // Get current and next vertices of the polygon to define an edge
            Vector2 vectorA = vertices[i];
            Vector2 vectorB = vertices[(i + 1) % vertices.Length];

            // Calculate the edge vector and its normal (perpendicular) axis
            Vector2 edge = vectorB - vectorA;
            axis = new Vector2(-edge.y, edge.x);
            axis.Normalize();

            // Project vertices of the polygon and the circle onto the separation axis
            Collisions.ProjectVertices(vertices, axis, out minA, out maxA);
            Collisions.ProjectCircle(circleCenter, circleRadius, axis, out minB, out maxB);

            // Check for overlap between the projections
            if (minA >= maxB || minB >= maxA)
            {
                // If there is no overlap, polygons are separated along this axis, return false
                return false;
            }

            // Calculate the depth of penetration along the separation axis
            axisDepth = Mathf.Min(maxB - minA, maxA - minB);

            // Update the minimum depth and corresponding normal if the depth is smaller
            if (axisDepth < depth)
            {
                depth = axisDepth;
                normal = axis;
            }
        }

        // Find the closest point on the polygon to the circle's center
        int cpIndex = Collisions.FindClosestPointToPolygon(circleCenter, vertices);
        Vector2 cp = vertices[cpIndex];

        // Calculate the axis towards the closest point
        axis = cp - circleCenter;
        axis.Normalize();

        // Project vertices of the polygon and the circle onto the separation axis
        Collisions.ProjectVertices(vertices, axis, out minA, out maxA);
        Collisions.ProjectCircle(circleCenter, circleRadius, axis, out minB, out maxB);

        // Check for overlap between the projections
        if (minA >= maxB || minB >= maxA)
        {
            // If there is no overlap, polygons are separated along this axis, return false
            return false;
        }

        // Calculate the depth of penetration along the separation axis
        axisDepth = Mathf.Min(maxB - minA, maxA - minB);

        // Update the minimum depth and corresponding normal if the depth is smaller
        if (axisDepth < depth)
        {
            depth = axisDepth;
            normal = axis;
        }

        // Adjust the normal vector based on the direction from the circle's center to the polygon's center
        Vector2 direction = polygonCenter - circleCenter;
        if (Mathf.Dot(direction, normal) < 0f)
        {
            normal = -normal;
        }

        // Collision detected, return true
        return true;
    }


    // Used if the center of the polygon is not passed
    public static bool IntersectCirclePolygon(Vector2 circleCenter, float circleRadius,
                                                Vector2[] vertices, out Vector2 normal, out float depth)
    {
        normal = new Vector2();
        // Put max in since we wanna find the lowest value to resolve the collision and so every value put in here will be always lower then the max one
        depth = float.MaxValue;

        Vector2 axis = new Vector2();
        float axisDepth = 0;
        float minA, maxA, minB, maxB = 0;
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector2 vectorA = vertices[i];
            Vector2 vectorB = vertices[(i + 1) % vertices.Length];

            Vector2 edge = vectorB - vectorA;
            // The axis used to check for separation, the normal of each edge
            axis = new Vector2(-edge.y, edge.x);
            axis.Normalize();

            Collisions.ProjectVertices(vertices, axis, out minA, out maxA);
            Collisions.ProjectCircle(circleCenter, circleRadius, axis, out minB, out maxB);

            if (minA >= maxB || minB >= maxA)
            {
                return false;
            }

            axisDepth = Mathf.Min(maxB - minA, maxA - minB);

            if (axisDepth < depth)
            {
                depth = axisDepth;
                normal = axis;
            }
        }

        int cpIndex = Collisions.FindClosestPointToPolygon(circleCenter, vertices);
        Vector2 cp = vertices[cpIndex];

        axis = cp - circleCenter;
        axis.Normalize();

        Collisions.ProjectVertices(vertices, axis, out minA, out maxA);
        Collisions.ProjectCircle(circleCenter, circleRadius, axis, out minB, out maxB);

        if (minA >= maxB || minB >= maxA)
        {
            return false;
        }

        axisDepth = Mathf.Min(maxB - minA, maxA - minB);

        if (axisDepth < depth)
        {
            depth = axisDepth;
            normal = axis;
        }

        Vector2 polygonCenter = Collisions.FindArithmeticCenter(vertices);

        Vector2 direction = polygonCenter - circleCenter;

        if (Mathf.Dot(direction, normal) < 0f)
        {
            normal = -normal;
        }

        return true;
    }

    private static int FindClosestPointToPolygon(Vector2 circleCenter, Vector2[] vertices)
    {
        // Initialize variables to store the index of the closest point and the minimum distance
        int result = -1;               // Index of the closest point (initialized to -1)
        float minDistance = float.MaxValue;  // Minimum distance (initialized to maximum possible value)

        // Loop through each vertex of the polygon
        for (int i = 0; i < vertices.Length; i++)
        {
            // Get the current vertex
            Vector2 v = vertices[i];

            // Calculate the distance between the current vertex and the circle's center
            float distance = Mathf.Distance(v, circleCenter);

            // Check if the current distance is smaller than the minimum distance found so far
            if (distance < minDistance)
            {
                // Update the minimum distance and the index of the closest point
                minDistance = distance;
                result = i;
            }
        }

        // Return the index of the closest point
        return result;
    }


    private static void ProjectCircle(Vector2 center, float radius, Vector2 axis, out float min, out float max)
    {
        // Normalize the axis to ensure it has unit length
        Vector2 direction = axis.Normalized();

        // Calculate the vector representing the direction of the axis multiplied by the radius of the circle
        Vector2 directionAndRadius = direction * radius;

        // Calculate the two points on the circle's circumference projected onto the axis
        Vector2 p1 = center + directionAndRadius;  // Point at the positive end of the axis
        Vector2 p2 = center - directionAndRadius;  // Point at the negative end of the axis

        // Project the points onto the axis and assign them to min and max
        min = Mathf.Dot(p1, axis);  // Projection of the positive point onto the axis
        max = Mathf.Dot(p2, axis);  // Projection of the negative point onto the axis

        // Ensure that min and max are ordered correctly (min should be less than or equal to max)
        if (min > max)
        {
            // Swap the values if min is greater than max
            float t = min;
            min = max;
            max = t;
        }
    }

}