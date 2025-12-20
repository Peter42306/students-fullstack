import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { createStudent } from "../../api/studentsApi";

export default function StudentCreate() {

    const navigate = useNavigate();
    
    const [form, setForm] = useState({
        firstName: '',
        lastName: '',
        email: '',
        dateOfBirth: '',
        enrollmentDate: '',
        notes: '',
    });

    const [saving, setSaving] = useState(false);
    const [error, setError] = useState('');

    function handleChange(e) {
        const {name, value} = e.target;
        setForm(prev => ({
            ...prev,
            [name]: value,
        }));
    }

    async function handleSubmit(e) {
        e.preventDefault();

        setSaving(true);
        setError('');

        try {
            await createStudent(form);
            navigate('/students');
        } catch (err) {
            setError(err?.message ?? 'Create failed');
        } finally {
            setSaving(false);
        }
    }

    return(
        <div className="container py-3">
            <h1>Students</h1>            
            <hr/>


            <div className="text-end mb-2">
                <Link to="/students" className="btn btn-outline-primary mb-2">
                    Back to list
                </Link>
            </div>            

            {error && (
                <div>
                    {error}
                </div>
            )}

            <form onSubmit={handleSubmit}>
                <div className="row g-3">
                    <div className="col-md-6">
                        <label>First Name</label>
                        <input
                            className="form-control"
                            name="firstName"
                            value={form.firstName}
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
                            value={form.lastName}
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
                            value={form.email}
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
                            value={form.dateOfBirth}
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
                            value={form.enrollmentDate}
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
                            value={form.notes}
                            onChange={handleChange}
                            maxLength={4000}                            
                        />
                    </div>

                    <div className="d-flex justify-content-end gap-2">
                        <button
                            className="btn btn-outline-primary d-flex align-items-center justify-content-center"
                            disabled={saving}
                            style={{minWidth:120}}
                        >
                            {saving && (
                                <span
                                    className="spinner-border spinner-border-sm"
                                    role="status"
                                    aria-hidden="true"
                                />
                            )}
                            <span>{saving ? "Saving..." : "Save"}</span>
                        </button>
                        <button
                            type="button"
                            className="btn btn-outline-secondary"
                            onClick={() => navigate('/students')}
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