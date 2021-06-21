using Fusee.Base.Common;
using Fusee.Base.Core;
using Fusee.Engine.Common;
using Fusee.Engine.Core;
using Fusee.Engine.Core.Effects;
using Fusee.Engine.Core.Scene;
using Fusee.Math.Core;
using Fusee.Serialization;
using Fusee.Xene;
using static Fusee.Engine.Core.Input;
using static Fusee.Engine.Core.Time;
using Fusee.Engine.GUI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FuseeApp
{
    [FuseeApplication(Name = "Tut08_FirstSteps", Description = "Yet another FUSEE App.")]
    public class Tut08_FirstSteps : RenderCanvas
    {
        private SceneContainer _scene;
        private SceneRendererForward _sceneRenderer;
        private Transform _cubeTransform;
        private Transform _cubeTransform2;
        private DefaultSurfaceEffect _cubeShader;
   //     private DefaultSurfaceEffect _cubeShader2;

        public int visibility = 0;


        // Init is called on startup. 
        public override void Init()
        {
            // Set the clear color for the backbuffer to "greenery"
            RC.ClearColor = (float4) ColorUint.Greenery;




            /*
            // Create a scene with a cube
            // The three components: one Transform, one ShaderEffect (blue material) and the Mesh
            _cubeTransform = new Transform {
                Translation = new float3(0, 0, 20),
                Rotation = new float3(0, 0.5f, 0)
            };
            _cubeShader = MakeEffect.FromDiffuseSpecular((float4)ColorUint.Blue, float4.Zero);
            var cubeMesh = SimpleMeshes.CreateCuboid(new float3(8, 8, 8));
            
            // Assemble the cube node containing the three components
            var cubeNode = new SceneNode();
            cubeNode.Components.Add(_cubeTransform);
            cubeNode.Components.Add(_cubeShader);
            cubeNode.Components.Add(cubeMesh);

            // Create the scene containing the cube as the only object
            _scene = new SceneContainer();
            _scene.Children.Add(cubeNode);

            // Create a scene renderer holding the scene above
            _sceneRenderer = new SceneRendererForward(_scene);

            */


            _cubeTransform = new Transform {
                Translation = new float3(0, 0, 20),
                Rotation = new float3(0, 0.5f, 0)
            };
            _cubeShader = MakeEffect.FromDiffuseSpecular((float4)ColorUint.Blue, float4.Zero);

            _cubeTransform2 = new Transform {
                Translation = new float3(0, 0, 20),
                Rotation = new float3(0, 0.5f, 0)
            };



            _scene = new SceneContainer { 
                Children = {
                    new SceneNode {
                        Components = {
                            _cubeTransform, _cubeShader, SimpleMeshes.CreateCuboid(new float3(6, 6, 6))
                        }
                    },
                    new SceneNode {
                        Components = { 
                            _cubeTransform2, _cubeShader, SimpleMeshes.CreateCuboid(new float3(4, 4, 4))
                        }
                    },
                    
                }
             };

             _sceneRenderer = new SceneRendererForward(_scene);
        }

        

        // RenderAFrame is called once a frame
        public override void RenderAFrame()
        {
            SetProjectionAndViewport();

            // Clear the backbuffer
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);


            _cubeTransform.Rotation = _cubeTransform.Rotation + new float3(0.01f, 0.01f, 0.01f);

            _cubeShader.SurfaceInput.Albedo = new float4(0.5f + 0.5f * M.Sin(2*Time.TimeSinceStart), 0, 0, 1);

            _cubeTransform2.Rotation = - _cubeTransform.Rotation + new float3(0.01f, 0.01f, 0.01f);

            

            if(Keyboard.WSAxis > 0 || Keyboard.WSAxis < 0) {
                _cubeTransform.Translation.y = _cubeTransform.Translation.y + (Keyboard.WSAxis * 0.25f);
            }

            if(Keyboard.ADAxis > 0 || Keyboard.ADAxis < 0) {
                _cubeTransform.Translation.x = _cubeTransform.Translation.x + (Keyboard.ADAxis * 0.25f);
            }

            if(_cubeTransform.Translation.x > 25) {
                _cubeTransform.Translation.x = -15;
            }
            if(_cubeTransform.Translation.x < -25) {
                _cubeTransform.Translation.x = 15;
            }

            if(_cubeTransform.Translation.y > 15) {
                _cubeTransform.Translation.y = -15;
            }
            if(_cubeTransform.Translation.y < -15) {
                _cubeTransform.Translation.y = 15;
            }

            if(Keyboard.IsKeyDown(KeyCodes.Back)) {
                _cubeTransform.Translation.x = 0;
                _cubeTransform.Translation.y = 0;

                _cubeTransform2.Translation.x = 0;
            }

           if(Keyboard.IsKeyDown(KeyCodes.Space)) {
                _cubeTransform2.Translation.x = _cubeTransform2.Translation.x +  5;
            }

            if(_cubeTransform2.Translation.x >= 25) {
                _cubeTransform2.Translation.x = -25;
            }

            

            // Render the scene on the current render context
            _sceneRenderer.Render(RC);





           // Swap buffers: Show the contents of the backbuffer (containing the currently rendered frame) on the front buffer.
            Present();
        }

        public void SetProjectionAndViewport()
        {
            // Set the rendering area to the entire window size
            RC.Viewport(0, 0, Width, Height);

            // Create a new projection matrix generating undistorted images on the new aspect ratio.
            var aspectRatio = Width / (float)Height;

            // 0.25*PI Rad -> 45° Opening angle along the vertical direction. Horizontal opening angle is calculated based on the aspect ratio
            // Front clipping happens at 1 (Objects nearer than 1 world unit get clipped)
            // Back clipping happens at 2000 (Anything further away from the camera than 2000 world units gets clipped, polygons will be cut)
            var projection = float4x4.CreatePerspectiveFieldOfView(M.PiOver4, aspectRatio, 1, 20000);
            RC.Projection = projection;
        }        

    }
}