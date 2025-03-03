﻿// Copyright 2020 The Tilt Brush Authors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using UnityEngine;

namespace TiltBrush
{
    public class SketchbookButton : OptionButton
    {
        [SerializeField] private float m_AdjustDistanceAmount;
        [SerializeField] private Renderer m_IconRenderer;

        override public void UpdateVisuals()
        {
            // this is ugly but for some reason even if this is set to true on the option button it gets turned to false 
            m_AllowUnavailable = true;
            base.UpdateVisuals();

        }

        protected override void AdjustButtonPositionAndScale(
            float posAmount, float scaleAmount, float boxColliderGrowAmount)
        {
            SetMaterialFloat("_Distance", posAmount != 0 ? m_AdjustDistanceAmount : 0.0f);
            SetMaterialFloat("_Grayscale", posAmount != 0 ? 0.0f : 1.0f);
            base.AdjustButtonPositionAndScale(posAmount, scaleAmount, boxColliderGrowAmount);
        }

        protected override void SetMaterialFloat(string name, float value)
        {
            m_IconRenderer.material.SetFloat(name, value);
        }
    }
} // namespace TiltBrush
