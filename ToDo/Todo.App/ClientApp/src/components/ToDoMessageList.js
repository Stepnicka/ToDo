import React from 'react'
import ToDoMessage from './ToDoMessage'

const ToDoMessageList = ({ messages }) => {

    return(

        <div>
            {messages.map(message => {
                return <ToDoMessage key={message.Key} message={message}/>
            })}
        </div>
        
    )
}

export default ToDoMessageList