using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class Controller
{
    private Model _model;
    private Vector3 _dir = new Vector3();

    private View _view;


    public Controller(Model model, View view)
    {
        _model = model;
        _view = view;
    }

    public void ArtificialUpdate()
    {
        _view.Jumping(false);

        if (Input.GetButtonDown("Jump"))
        {
            _view.Jumping(true);
            _model.Jump();
        }

        if (Input.GetButtonUp("Jump"))
        {
            _view.Jumping(false);
            _model.CutJump();
        }     

    }

    public void ListenFixedKeys()
    {

        _dir.x = Input.GetAxisRaw("Horizontal");
        _dir.z = Input.GetAxisRaw("Vertical");

        //Le paso al view para donde me estoy moviendo
        _view.Walking(_dir);
        //Llamo el metodo de movimiento del Model
        _model.Movement(_dir);

    }
}
