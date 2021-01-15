using OpenTK.Graphics.ES30;
using RoboRemote.Classes;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RoboRemote.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class _3DView : ContentPage
    {
        float red, green, blue;
        public _3DView()
        {
            InitializeComponent();

            Title = "OpenGL";
            var view = new OpenGLView { HasRenderLoop = true };
            var toggle = new Switch { IsToggled = true };
            var button = new Button { Text = "Display" };

            view.HeightRequest = 300;   
            view.WidthRequest = 300;

            view.OnDisplay = r => {

                GL.ClearColor(red, green, blue, 1.0f);
                GL.Clear((ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit));
                
                red += 0.01f;
                if (red >= 1.0f)
                    red -= 1.0f;
                green += 0.02f;
                if (green >= 1.0f)
                    green -= 1.0f;
                blue += 0.03f;
                if (blue >= 1.0f)
                    blue -= 1.0f;
            };

            toggle.Toggled += (s, a) => {
                view.HasRenderLoop = toggle.IsToggled;
            };
            button.Clicked += (s, a) => view.Display();

            var stack = new StackLayout
            {
                Padding = new Size(20, 20),
                Children = { view, toggle, button }
            };

            Content = stack;
        }

        //private void BuildRobot()
        //{
        //    //using (SimpleDataBase mod = new SimpleDataBase("Resources\\Resources.sqlite", "Models"))
        //    //{
        //    JSONMeshLabModel.Rootobject obj = StaticUtils.Json.ReadObjectFromFile<JSONMeshLabModel.Rootobject>(@"Resources\\LD_geo.json");
        //    //JSONMeshLabModel.Rootobject obj = new JSONMeshLabModel.Rootobject();
        //    //obj = mod.GetValue("LD_geo", obj);

        //    var vertices = new List<JSONMeshLabModel.Vertex>();
        //    var indices = new List<int>();

        //    //int vCount = obj.vertices[0].values.Length / obj.vertices[0].size;
        //    for (int i = 0; i < obj.vertices[0].values.Length; i += obj.vertices[0].size)
        //    {
        //        vertices.Add(new Vertex
        //        {
        //            Pos = new Vector3(
        //                    Convert.ToSingle(obj.vertices[0].values[i], CultureInfo.InvariantCulture),
        //                    Convert.ToSingle(obj.vertices[0].values[i + 1], CultureInfo.InvariantCulture),
        //                    Convert.ToSingle(obj.vertices[0].values[i + 2], CultureInfo.InvariantCulture)),
        //            Normal = new Vector3(
        //                    Convert.ToSingle(obj.vertices[1].values[i], CultureInfo.InvariantCulture),
        //                    Convert.ToSingle(obj.vertices[1].values[i + 1], CultureInfo.InvariantCulture),
        //                    Convert.ToSingle(obj.vertices[1].values[i + 2], CultureInfo.InvariantCulture))
        //        });
        //    }

        //    foreach (int i in obj.connectivity[0].indices)
        //    {
        //        indices.Add(i);
        //    }
        //}
        }
}