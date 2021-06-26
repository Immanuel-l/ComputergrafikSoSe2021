using Fusee.Base.Common;
using Fusee.Base.Core;
using Fusee.Engine.Common;
using Fusee.Engine.Core;
using Fusee.Engine.Core.Scene;
using Fusee.Engine.Core.Effects;
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
    [FuseeApplication(Name = "Tut11_AssetsPicking", Description = "Yet another FUSEE App.")]
    public class Tut11_AssetsPicking : RenderCanvas
    {
        private SceneContainer _scene;

        private SceneRendererForward _sceneRenderer;
        private Transform _body;
        private Transform _achseVorneLinksTransform;
        private Transform _achseMitteLinksTransform;
        private Transform _achseHintenLinksTransform;
        private Transform _achseVorneRechtsTransform;
        private Transform _achseMitteRechtsTransform;
        private Transform _achseHintenRechtsTransform;
        private Transform _kopfHalterungTransform;
        private Transform _kopfNeigungTransform;
        private Transform _greifArmObenTransform;
        private Transform _greifArmUntenTransform;

        private SurfaceEffect _bodySurfaceEffect;
        private SurfaceEffect _achseVorneLinksSurfaceEffect;
        private SurfaceEffect _achseMitteLinksSurfaceEffect;
        private SurfaceEffect _achseHintenLinksSurfaceEffect;
        private SurfaceEffect _achseVorneRechtsSurfaceEffect;
        private SurfaceEffect _achseMitteRechtsSurfaceEffect;
        private SurfaceEffect _achseHintenRechtsSurfaceEffect;
        private SurfaceEffect _kopfHalterungSurfaceEffect;
        private SurfaceEffect _kopfNeigungSurfaceEffect;


        

        private ScenePicker _scenePicker;
        private PickResult _currentPick;
        private float4 _oldColor;


        private float _camAngleX = 0; 
        private float _camAngleY = 0.25f;
        private float _camAngleZ = 0;
        private float _oldpoition = 0;
        private float _zoom = 10;



        // Init is called on startup. 
        public override void Init()
        {
            RC.ClearColor = new float4(0.8f, 0.9f, 0.7f, 1);

            _oldpoition = Mouse.Position.x;

            _scene = AssetStorage.Get<SceneContainer>("MarsRover.fus");

            _body = _scene.Children.FindNodes(node => node.Name == "Body")?.FirstOrDefault()?.GetTransform();
            _bodySurfaceEffect = _scene.Children.FindNodes(node => node.Name == "Body")?.FirstOrDefault()?.GetComponent<SurfaceEffect>();

            _achseVorneLinksTransform = _scene.Children.FindNodes(node => node.Name == "AchseVorneLinks")?.FirstOrDefault()?.GetTransform();
            _achseVorneLinksSurfaceEffect = _scene.Children.FindNodes(node => node.Name == "AchseVorneLinks")?.FirstOrDefault()?.GetComponent<SurfaceEffect>();

            _achseMitteLinksTransform = _scene.Children.FindNodes(node => node.Name == "AchseMitteLinks")?.FirstOrDefault()?.GetTransform();
            _achseMitteLinksSurfaceEffect = _scene.Children.FindNodes(node => node.Name == "AchseMitteLinks")?.FirstOrDefault()?.GetComponent<SurfaceEffect>();

            _achseHintenLinksTransform = _scene.Children.FindNodes(node => node.Name == "AchseHintenLinks")?.FirstOrDefault()?.GetTransform();
            _achseHintenLinksSurfaceEffect = _scene.Children.FindNodes(node => node.Name == "AchseHintenLinks")?.FirstOrDefault()?.GetComponent<SurfaceEffect>();

            _achseVorneRechtsTransform = _scene.Children.FindNodes(node => node.Name == "AchseVorneRechts")?.FirstOrDefault()?.GetTransform();
            _achseVorneRechtsSurfaceEffect = _scene.Children.FindNodes(node => node.Name == "AchseVorneRechts")?.FirstOrDefault()?.GetComponent<SurfaceEffect>();

            _achseMitteRechtsTransform = _scene.Children.FindNodes(node => node.Name == "AchseMitteRechts")?.FirstOrDefault()?.GetTransform();
            _achseMitteRechtsSurfaceEffect = _scene.Children.FindNodes(node => node.Name == "AchseMitteRechts")?.FirstOrDefault()?.GetComponent<SurfaceEffect>();

            _achseHintenRechtsTransform = _scene.Children.FindNodes(node => node.Name == "AchseHintenRechts")?.FirstOrDefault()?.GetTransform();
            _achseHintenRechtsSurfaceEffect = _scene.Children.FindNodes(node => node.Name == "AchseHintenRechts")?.FirstOrDefault()?.GetComponent<SurfaceEffect>();

            _greifArmObenTransform = _scene.Children.FindNodes(node => node.Name == "GreifArmOben")?.FirstOrDefault()?.GetTransform();

            _greifArmUntenTransform = _scene.Children.FindNodes(node => node.Name == "GreifArmUnten")?.FirstOrDefault()?.GetTransform();


            _kopfHalterungTransform = _scene.Children.FindNodes(node => node.Name == "KopfHalterung")?.FirstOrDefault()?.GetTransform();
            _kopfHalterungSurfaceEffect = _scene.Children.FindNodes(node => node.Name == "KopfHalterung")?.FirstOrDefault()?.GetComponent<SurfaceEffect>();

            _kopfNeigungTransform = _scene.Children.FindNodes(node => node.Name == "KopfNeigung")?.FirstOrDefault()?.GetTransform();
            _kopfNeigungSurfaceEffect = _scene.Children.FindNodes(node => node.Name == "KopfNeigung")?.FirstOrDefault()?.GetComponent<SurfaceEffect>();



            // Create a scene renderer holding the scene above
            _sceneRenderer = new SceneRendererForward(_scene);
            _scenePicker = new ScenePicker(_scene);

        }

        // RenderAFrame is called once a frame
        // RenderAFrame is called once a frame
        public override void RenderAFrame()
        {
            SetProjectionAndViewport();
            _body.Translation.z += Keyboard.WSAxis * Time.DeltaTime * 5;

            _achseVorneLinksTransform.Rotation.x += Keyboard.WSAxis * Time.DeltaTime * 2;
            _achseMitteLinksTransform.Rotation.x += Keyboard.WSAxis * Time.DeltaTime * 2;
            _achseHintenLinksTransform.Rotation.x += Keyboard.WSAxis * Time.DeltaTime * 2;

            _achseVorneRechtsTransform.Rotation.x += Keyboard.WSAxis * Time.DeltaTime * 2;
            _achseMitteRechtsTransform.Rotation.x += Keyboard.WSAxis * Time.DeltaTime * 2;
            _achseHintenRechtsTransform.Rotation.x += Keyboard.WSAxis * Time.DeltaTime * 2;



            if(_kopfNeigungTransform.Rotation.x < 1.7) {
                _kopfNeigungTransform.Rotation.x += (Keyboard.UpDownAxis * Time.DeltaTime);
                if(_kopfNeigungTransform.Rotation.x > 1.7) {
                    _kopfNeigungTransform.Rotation.x -= (Keyboard.UpDownAxis * Time.DeltaTime);
                }
            }
            if(_kopfNeigungTransform.Rotation.x < 0.6) {
                _kopfNeigungTransform.Rotation.x -= (Keyboard.UpDownAxis * Time.DeltaTime);
            }

            _kopfHalterungTransform.Rotation.y += Keyboard.ADAxis * Time.DeltaTime * 2;

            if(_greifArmObenTransform.Rotation.y > -1.8) {
                _greifArmObenTransform.Rotation.y += Keyboard.LeftRightAxis * Time.DeltaTime * 2;
                _greifArmUntenTransform.Rotation.y -= Keyboard.LeftRightAxis * Time.DeltaTime * 2;
                if(_greifArmObenTransform.Rotation.y < -1.8) {
                    _greifArmObenTransform.Rotation.y -= Keyboard.LeftRightAxis * Time.DeltaTime * 2;
                    _greifArmUntenTransform.Rotation.y += Keyboard.LeftRightAxis * Time.DeltaTime * 2;
                }
            }
            if(_greifArmObenTransform.Rotation.y > -0.9) {
                _greifArmObenTransform.Rotation.y -= Keyboard.LeftRightAxis * Time.DeltaTime * 2;
                _greifArmUntenTransform.Rotation.y += Keyboard.LeftRightAxis * Time.DeltaTime * 2;
            }


            


            
            if (Mouse.LeftButton)
            {
                float2 pickPosClip = Mouse.Position * new float2(2.0f / Width, -2.0f / Height) + new float2(-1, 1);

                PickResult newPick = _scenePicker.Pick(RC, pickPosClip).OrderBy(pr => pr.ClipPos.z).FirstOrDefault();

                if (newPick?.Node != _currentPick?.Node)
                {
                    if (_currentPick != null)
                    {
                        var ef = _currentPick.Node.GetComponent<DefaultSurfaceEffect>();
                        ef.SurfaceInput.Albedo = _oldColor;
                    }
                    if (newPick != null) {  
                        var SurfaceEffect = newPick.Node.GetComponent<SurfaceEffect>();
                        var Transform = newPick.Node.GetTransform();
                        _oldColor = SurfaceEffect.SurfaceInput.Albedo;
                        SurfaceEffect.SurfaceInput.Albedo = (float4) ColorUint.DarkOrchid;

                        if(newPick.Node.Name == "ReifenMitteLinks") {
                            SurfaceEffect.SurfaceInput.Albedo = (float4) ColorUint.DarkRed;
                            Transform.Rotation.x -= Keyboard.WSAxis * Time.DeltaTime * 2;
                        }

                        if(newPick.Node.Name == "Body") {
                            SurfaceEffect.SurfaceInput.Albedo = (float4) ColorUint.Silver;
                             Transform.Translation.z += 10;
                        }

                        Diagnostics.Debug(newPick.Node.Name);

                        
    
                    }
                    _currentPick = newPick;
                }

            }




            


        //     if(Keyboard.GetKey(KeyCodes.B)) {
        //     _rightRearWhellSurfaceEffect.SurfaceInput.Albedo = (float4) ColorUint.DarkRed;
        //     _rightRearWhellTransform.Rotation = new float3(-M.MinAngle(TimeSinceStart), 0, 0);
        //     }else {
        //     _rightRearWhellSurfaceEffect.SurfaceInput.Albedo = (float4) ColorUint.DarkBlue;
        //    _rightRearWhellTransform.Rotation = new float3(M.MinAngle(TimeSinceStart), 0, 0);
        //     }
            // _rightRearWhellTransform.Rotation = new float3(M.MinAngle(TimeSinceStart), 0, 0);
            // _leftRearWhellTransform.Rotation = new float3(M.MinAngle(TimeSinceStart), 0, 0);
            // _rightFrontWhellTransform.Rotation = new float3(M.MinAngle(TimeSinceStart), 0, 0);
            // _leftFrontWhellTransform.Rotation = new float3(M.MinAngle(TimeSinceStart), 0, 0);

            // if(Mouse.LeftButton) {
            //     float2 pickPosClip = Mouse.Position * new float2(2.0f / Width, -2.0f / Height) + new float2(-1, 1);
            //     PickResult newPick = _scenePick.Pick(RC, pickPosClip).OrderBy(pr => pr.ClipPos.z).FirstOrDefault();
            //     if (newPick != null) {
            //         Diagnostics.Debug(newPick.Node.Name);
            //     }
            // }

           if(Mouse.LeftButton) {
                _camAngleX += Mouse.Velocity.x * 0.00005f;
                _camAngleY += Mouse.Velocity.y * -0.00005f;
            }
            if(Mouse.Wheel > 0) {
                _zoom = 5 - (Mouse.Wheel * 5);
            }
            if (Mouse.Wheel < 0) {
                _zoom = 5 + (-Mouse.Wheel * 5);
            }

            // Clear the backbuffer
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);

            // Setup the camera 
            RC.View = float4x4.CreateTranslation(0, 0, _zoom) * float4x4.CreateRotationYXZ(-_camAngleY, -_camAngleX, -_camAngleZ);

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