import React from 'react'
import {ToDoListItem} from './ToDoListItem'

const TodoList = ({tasks, states, markEdit, updateTask, deleteTask}) => {

    return(
        <table className='table'>
            <thead>
                <tr>
                    <th scope='col'>Id</th>
                    <th scope='col'>Name</th>
                    <th scope='col'>Priority</th>
                    <th scope='col'>State</th>
                    <th scope='col'></th>
                    <th scope='col'></th>
                </tr>
            </thead>
            <tbody>
                {tasks.map(task => {
                    return <ToDoListItem key={task.toDoTaskId} task={task} states={states} markEdit={markEdit} updateTask={updateTask} deleteTask={deleteTask}/>
                })}
            </tbody>
        </table>
    )
}

export default TodoList