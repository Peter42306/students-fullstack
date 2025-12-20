import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { createStudent } from "../../api/studentsApi";
import StudentForm from "../../components/students/StudentForm";

const empty = {firstName:'', lastName:'', email:'', dateOfBirth:'', enrollmentDate:'', notes:'',};

export default function StudentCreate() {

    const navigate = useNavigate();    

    const [form, setForm] = useState({empty});
    const [saving, setSaving] = useState(false);
    const [error, setError] = useState('');    

    async function onSubmit(e) {
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
            <div className="d-flex justify-content-between align-items-center">
                <h1 className="mb-0">Students</h1>            
                <div className="text-end">
                    <Link 
                        to="/students" 
                        className="btn btn-outline-primary btn-fixed"                    
                    >
                        Back to list
                    </Link>
                </div>            
            </div>            
            <hr/>

            <StudentForm 
                value={form}
                onChange={setForm}
                onSubmit={onSubmit}
                onCancel={() => navigate('/students')}
                saving={saving}
                error={error}
                submitText="Save"
            />
        </div>
    );
}