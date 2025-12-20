export default function StudentsError({error}) {
    return(
        <div className="alert alert-danger">
            {error}
        </div>
    );
}