using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;

namespace WF
{

    public sealed class ReadInt : NativeActivity<int>
    {
        // Defina un argumento de entrada de actividad de tipo string
        [RequiredArgument]
        public InArgument<string> BookMarkName { get; set; }

        // Si la actividad devuelve un valor, se debe derivar de CodeActivity<TResult>
        // y devolver el valor desde el método Execute.
        protected override void Execute(NativeActivityContext context)
        {
            // Obtenga el valor de tiempo de ejecución del argumento de entrada Text
            string name = BookMarkName.Get(context);
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("BookmarkName no puede estar vacío", "BookmarkName");
            
            }
            context.CreateBookmark(name, new BookmarkCallback(OnReadComplete));
        }

        // NativeActivity derived activities that do asynchronous operations by calling
        // one of the CreateBookmark overloads defined on System.Activities.NativeActivityContext
        // must override the CanInduceIdle property and return true.
        protected override bool CanInduceIdle
        {
            get { return true; }
        }

        void OnReadComplete(NativeActivityContext context, Bookmark bookmark, object state)
        {
            this.Result.Set(context, Convert.ToInt32(state));
        }

    }
}
