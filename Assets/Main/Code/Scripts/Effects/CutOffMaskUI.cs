using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class CutOffMaskUI : Image
{
    private Material _customMaterial;

    public override Material materialForRendering
    {
        get
        {
            if (_customMaterial == null)
            {
                _customMaterial = new Material(base.materialForRendering);
                _customMaterial.SetInt("_StencilComp", (int)CompareFunction.NotEqual);
            }
            return _customMaterial;
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (_customMaterial != null)
        {
            Destroy(_customMaterial); // Upewnij siê, ¿e materia³ zostanie zwolniony.
        }
    }
}
