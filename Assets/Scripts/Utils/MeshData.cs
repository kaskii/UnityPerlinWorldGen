using System.Collections.Generic;
using Map.Texturing;
using UnityEngine;

namespace Utils
{
    public class MeshData
    {
        public Mesh CurrentMesh
        {
            get
            {
                if (_meshInitialized) return _mesh;

                _mesh.Clear(false);

                _mesh.vertices = _verts.ToArray();
                _mesh.triangles = _tris.ToArray();
                _mesh.uv = _uvs.ToArray();

                _mesh.RecalculateNormals();
                _mesh.RecalculateTangents();
                _mesh.MarkDynamic();

                _meshInitialized = true;

                return _mesh;
            }
        }

        private readonly Mesh _mesh;

        private readonly List<Vector3> _verts;
        private readonly List<int> _tris;
        private readonly List<Vector2> _uvs;

        private bool _meshInitialized;

        public MeshData()
        {
            _verts = new List<Vector3>();
            _tris = new List<int>();
            _uvs = new List<Vector2>();

            _meshInitialized = false;
            _mesh = new Mesh();
            _mesh.MarkDynamic();
        }

        public void CreateFace(Vector3 corner, Vector3 up, Vector3 right, TileTexture tileTexture)
        {
            int tIndex = _verts.Count;

            _verts.Add(corner);
            _verts.Add(corner + up);
            _verts.Add(corner + up + right);
            _verts.Add(corner + right);

            _tris.Add(tIndex + 0);
            _tris.Add(tIndex + 1);
            _tris.Add(tIndex + 2);
            _tris.Add(tIndex + 2);
            _tris.Add(tIndex + 3);
            _tris.Add(tIndex + 0);

            _uvs.AddRange(tileTexture.GetUVs());

        }

        public void Clear()
        {
            _verts.Clear();
            _tris.Clear();
            _uvs.Clear();

            _meshInitialized = false;
            _mesh.Clear(false);
        }
    }
}