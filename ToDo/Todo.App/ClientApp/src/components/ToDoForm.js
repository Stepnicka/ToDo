import React, { useRef } from 'react'

const ToDoForm = ({ addTask }) => {

    const taskNameRef = useRef()
    const taskPriorityRef = useRef()

    const handleTaskSubmit = async (e) => {
        e.preventDefault();

        if(await addTask({TaskName: taskNameRef.current.value, TaskPriority: taskPriorityRef.current.value}) === true){
            taskNameRef.current.value = null;
            taskPriorityRef.current.value = null;
        }
    }

    return (

        <form onSubmit={handleTaskSubmit}>
            <div className="form-row">
                <div className="col">
                    <label htmlFor="taskNameInput">Task name</label>
                    <input ref={taskNameRef} type="text" className="form-control" id="taskNameInput" maxLength={250}/>
                    <small className="form-text text-muted">Every task name must be unique</small>
                </div>
                <div className="col">
                    <label htmlFor="taskPriorityInput">Task priority</label>
                    <input ref={taskPriorityRef} type="number" className="form-control" id="taskPriorityInput" min="1" max="2147483647" />
                    <small className="form-text text-muted">Minimal priority is 1</small>
                </div>
            </div>
            <div className='form-group'>
                <button type='submit' className='btn btn-primary'>Add Task</button>
            </div>
        </form>

    )
}

export default ToDoForm