export default function StudentForm({    
    value,
    onChange,
    onSubmit,
    onCancel,
    saving = false,
    error = '',
    submitText = 'Save',
}) {
    function handleChange(e) {
        const {name, value: newValue} = e.target;
        onChange({...value, [name]: newValue});
    }

    return(        
        <div className="d-flex flex-column gap-3">            
            
            {error && <div className="alert alert-danger"> {error} </div> }

            <form onSubmit={onSubmit}>
                <div className="row g-3">
                    <div className="col-md-6">
                        <label>First Name</label>
                        <input
                            className="form-control"
                            name="firstName"
                            value={value.firstName}
                            onChange={handleChange}
                            maxLength={100}
                            required
                        />
                    </div>
                    <div className="col-md-6">
                        <label>Last Name</label>
                        <input
                            className="form-control"
                            name="lastName"
                            value={value.lastName}
                            onChange={handleChange}
                            maxLength={100}
                            required
                        />
                    </div>
                    <div className="col-md-4">
                        <label>Email</label>
                        <input
                            type="email"
                            className="form-control"
                            name="email"
                            value={value.email}
                            onChange={handleChange}
                            maxLength={255}
                            required
                        />
                    </div>
                    <div className="col-md-4">
                        <label>Date of birth</label>
                        <input
                            type="date"
                            className="form-control"
                            name="dateOfBirth"
                            value={value.dateOfBirth}
                            onChange={handleChange}
                            required
                        />
                    </div>
                    <div className="col-md-4">
                        <label>Enrollment date</label>
                        <input
                            type="date"
                            className="form-control"
                            name="enrollmentDate"
                            value={value.enrollmentDate}
                            onChange={handleChange}
                            required
                        />
                    </div>
                    <div className="col-12">
                        <label>Notes</label>
                        <textarea
                            className="form-control"
                            name="notes"
                            rows={4}
                            value={value.notes}
                            onChange={handleChange}
                            maxLength={4000}                            
                        />
                    </div>

                    <div className="d-flex justify-content-end gap-2">
                        <button
                            className="btn btn-outline-primary d-flex align-items-center justify-content-center btn-fixed"
                            disabled={saving}                            
                        >
                            {saving && (
                                <span
                                    className="spinner-border spinner-border-sm"
                                    role="status"
                                    aria-hidden="true"
                                />
                            )}
                            <span>{saving ? "Saving..." : submitText}</span>
                        </button>
                        <button
                            type="button"
                            className="btn btn-outline-secondary btn-fixed"
                            onClick={onCancel}
                            disabled={saving}                            
                        >
                            Cancel
                        </button>
                    </div>
                </div>
            </form>
        </div>        
    );
}