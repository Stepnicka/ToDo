import React from "react"

const ToDoMessage = ({message}) => {

    return (

        <div className="alert alert-danger" role="alert">
            {message.Text}
        </div>

    )

}

export default ToDoMessage