import { useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { deleteEnclosure, enclosureDownloadUrl, getEnclosures, getStudent, updateStudent, uploadEnclosures } from "../../api/studentsApi";
import StudentForm from "../../components/students/StudentForm";

export default function StudentUpdate() {

    const {id} = useParams();
    const navigate = useNavigate();

    const [form, setForm] = useState(null);
    const [loading, setLoading] = useState(true);
    const [saving, setSaving] = useState(false);
    const [error, setError] = useState('');

    const [enclosures, setEnclosures] = useState([]);
    const [filesToUpload, setFilesToUpload] = useState([]);
    const [uploading, setUploading] = useState(false);
    const [photosError, setPhotosError] = useState("");

    useEffect(() =>{
        (async () => {
            setLoading(true);
            setError('');

            try {
                const [student, enc] = await Promise.all([
                    getStudent(id),
                    getEnclosures(id),
                ]);                

                setForm({
                    firstName: student.firstName ?? '',
                    lastName: student.lastName ?? '',
                    email: student.email ?? '',
                    dateOfBirth: (student.dateOfBirth ?? '').slice(0, 10),
                    enrollmentDate: (student.enrollmentDate ?? '').slice(0, 10),
                    notes: student.notes ?? '',
                });

                setEnclosures(enc ?? []);
            } catch (err) {
                setError(err?.message ?? 'Load failed')
            } finally {
                setLoading(false);
            }
        })();
    }, [id]);

    async function handleSubmit(e) {
        e.preventDefault();

        if(!form){
            return;
        }

        setSaving(true);
        setError('');

        try {
            await updateStudent(id, form);
            navigate(`/students/${id}`);
        } catch (err) {
            setError(err?.message ?? 'Update failed')
        } finally {
            setSaving(false);
        }
    }

    function handleCancel() {
        navigate(`/students/${id}`)
    }

    async function handleUploadEnclosure() {
        setPhotosError("");

        if(filesToUpload.length === 0){
            return;
        }

        setUploading(true);

        try {
            await uploadEnclosures(id, filesToUpload);
            setFilesToUpload([]);

            const enc = await getEnclosures(id);
            setEnclosures(enc ?? []);

        } catch (err) {
            setPhotosError(err?.message ?? "Upload failed.")
        } finally {
            setUploading(false);
        }
    }

    async function handleDeleteEnclosure(enclosureId) {
        setPhotosError("");

        const ok = window.confirm("Are you sure you want to delete this photo?")
        if(!ok){
            return;
        }

        try {
            await deleteEnclosure(id, enclosureId);
            setEnclosures(prev => prev.filter(x => x.id !== enclosureId));
        } catch (err) {
            setPhotosError(err?.message ?? "Delete failed.");
        }
    }


    if(loading){
        return(
            <div className="container py-3">
                Loading...
            </div>
        );
    }
    if(error && !form){
        return(
            <div className="container py-3">
                <div className="alert alert-danger">
                    {error}
                </div>
            </div>
        );
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
                onSubmit={handleSubmit}
                onCancel={handleCancel}
                saving={saving}
                error={error}
                submitText="Update"
            />

            <hr/>            

            {photosError && <div className="alert alert-danger">{photosError}</div>}

            <div>
                <label>Add or delete enclosures</label>
                <input
                    type="file"
                    className="form-control"
                    multiple
                    disabled={saving || uploading}
                    onChange={(e) => setFilesToUpload(Array.from(e.target.files ?? []))}
                />

                {filesToUpload.length > 0 && (
                    <div className="mt-2">
                        Selected: {filesToUpload.length} files
                    </div>
                )}
            </div>
            <div className="text-end">
                <button
                    type="button"
                    className="btn btn-outline-primary btn-fixed mt-2"
                    disabled={saving || uploading || filesToUpload.length === 0}
                    onClick={handleUploadEnclosure}
                >
                    {uploading ? "Uploading..." : "Upload"}
                </button>
            </div>
            <hr/>

            {enclosures.length > 0 && (
                <div>
                    {enclosures.map((e) => (
                        <div key={e.id} className="mb-4">
                            <div className="position-relative">
                                <img
                                    src={enclosureDownloadUrl(id, e.id)}
                                    alt={e.fileName}
                                    className="img-fluid rounded border shadow"
                                />
                                <button 
                                    type="button"
                                    className="btn btn-danger btn-fixed position-absolute top-0 end-0 m-2"
                                    disabled={saving || uploading}
                                    onClick={() => handleDeleteEnclosure(e.id)}
                                    title="Delete photo"
                                >
                                    Delete
                                </button>
                            </div>                            
                        </div>
                    ))}
                </div>
            )}

        </div>
    );
    
}