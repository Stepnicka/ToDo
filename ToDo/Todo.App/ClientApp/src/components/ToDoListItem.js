import React, { Component, createRef } from 'react'
import Select from 'react-select'

export class ToDoListItem extends Component{
    static displayName = ToDoListItem.name;

    constructor(props) {
        super(props);
        this.state = { task: props.task, states: props.states };

        this.markEdit = props.markEdit;
        this.updateTask = props.updateTask;
        this.deleteTask = props.deleteTask;

        this.nameRef = createRef();
        this.priorityRef = createRef();
        this.stateRef = createRef();

        this.stateOptions = [];
        for(const item of this.state.states)
            this.stateOptions.push({
                value: item.taskStatusId,
                label: item.displayName
            })
    }

    togleUpdate = (e) => {
        e.preventDefault();
        this.markEdit(this.state.task.toDoTaskId)
    }

    cancelUpdate = (e) => {
        e.preventDefault();

        let newState = {...this.state}
        newState.task.edited = false;

        this.setState(newState);
    }

    saveUpdate = async (e) => {
        e.preventDefault();

        if(await this.updateTask({
            toDoTaskId: this.state.task.toDoTaskId,
            name: this.nameRef.current.value,
            priority: this.priorityRef.current.value,
            taskStatusId: this.stateRef.current.getValue()[0].value
        })  === true){

            let newState = {...this.state}
            newState.task.edited = false;
            newState.task.name = this.nameRef.current.value;
            newState.task.priority = this.priorityRef.current.value;
            newState.task.taskStatusId = this.stateRef.current.getValue()[0].value;
            this.setState(newState);
        }
    }

    delete = async (e) => {
        e.preventDefault();

        if(await this.deleteTask(this.state.task) === true){

        }
    }

    render(){

        let contents;

        if(this.state.task.edited !== true){
            contents = 
            <tr>
                <td className="align-middle">
                    {this.state.task.toDoTaskId}
                </td>
                <td className="align-middle">
                    {this.state.task.name}
                </td>
                <td className="align-middle">
                    {this.state.task.priority}
                </td>
                <td className="align-middle">
                    {this.state.states[this.state.task.taskStatusId-1].displayName}
                </td>
                <td className='p-2'>
                        <button type='button' className="btn btn-md btn-light mx-auto" onClick={this.togleUpdate}>Edit</button>
                </td>
                <td className='p-2'>
                    {this.state.task.taskStatusId === 3 ? 
                        <button type="button" className="btn btn-md btn-danger mx-auto" onClick={this.delete}>Delete</button>
                        : 
                        <div></div>
                    }
                </td>
            </tr>
        }
        else{
            contents = 
            <tr>
                <td className="align-middle">
                    {this.state.task.toDoTaskId}
                </td>
                <td className='p-2'>
                    <input ref={this.nameRef} defaultValue={this.state.task.name} type="text" className="form-control form-control-md" maxLength={250}/>
                </td>
                <td className='p-2'>
                    <input ref={this.priorityRef} defaultValue={this.state.task.priority} type="number" className="form-control form-control-md" min="1" max="2147483647" />
                </td>
                <td className='p-2'>
                    <Select
                        ref={this.stateRef}
                        className="basic-single"
                        classNamePrefix="select"
                        options={this.stateOptions}
                        defaultValue={this.stateOptions[this.state.task.taskStatusId-1]}
                        isClearable={false}
                        isSearchable={false}
                        name="color"
                    />
                </td>
                <td className='p-2'>
                    <button type="button" className="btn btn-md btn-primary mx-auto" onClick={this.saveUpdate}>Save</button>
                </td>
                <td className='p-2'>
                    <button type="button" className="btn btn-md btn-dark mx-auto" onClick={this.cancelUpdate}>Cancel</button>
                </td>
            </tr>
        }

        return (contents)
    }
}