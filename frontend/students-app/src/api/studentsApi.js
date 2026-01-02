const API_BASE = '/api/students';

async function handleResponse(res) {
    if(!res.ok){
        let message = res.statusText;

        try {
            const data = await res.json();
            message = data.message ?? message;
        } catch {
            // if not JSON remained message = res.statusText
        }

        throw new Error(message);
    }

    if(res.status === 204){
        return;
    }

    return res.json();
}

// GET /api/students
export async function getStudents(search, page, pageSize, sortBy, sortDirection) {
    const params = new URLSearchParams();

    if(search?.trim()){
        params.set('search', search.trim());
    }

    params.set('page', String(page));
    params.set('pageSize', String(pageSize));
    params.set('sortBy', sortBy);
    params.set('sortDirection', sortDirection);

    const res = await fetch(`${API_BASE}?${params.toString()}`);
    return handleResponse(res);
}

// /api/Students
export async function createStudent(dto) {
    const res = await fetch(API_BASE,{
        method:'POST',
        headers:{'Content-Type': 'application/json'},
        body:JSON.stringify(dto),
    });
    return handleResponse(res);
}

// /api/Students/{id}
export async function deleteStudent(id) {
    const res = await fetch(`${API_BASE}/${id}`,{method: 'DELETE'});
    if(res.status === 204){
        return;
    }
    return handleResponse(res);
}

// /api/Students/{id}
export async function getStudent(id) {
    const res = await fetch(`${API_BASE}/${id}`);
    return handleResponse(res);
}

// /api/Students/{id}
export async function updateStudent(id, dto) {
  const res = await fetch(`${API_BASE}/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(dto),
  });
  return handleResponse(res);
}

// POST /api/students/{id}/enclosures
export async function uploadEnclosures(studentId, files) {
    const form = new FormData();

    for(const file of files){
        form.append("Files", file);
    }

    const res = await fetch(`${API_BASE}/${studentId}/enclosures`,{
        method: "POST",
        body: form,
    });

    return handleResponse(res);
}

// GET /api/students/{id}/enclosures
export async function getEnclosures(studentId) {
    const res = await fetch(`${API_BASE}/${studentId}/enclosures`);
    return handleResponse(res);
}

// DELETE /api/students/{id}/enclosures/{enclosureId}
export async function deleteEnclosure(studentId, enclosureId) {
    const res = await fetch(`${API_BASE}/${studentId}/enclosures/${enclosureId}`,{
        method:"DELETE",
    });

    if(res.status === 204){
        return;
    }

    return handleResponse(res);
}

// GET /api/students/{id}/enclosures/{enclosureId}
export function enclosureDownloadUrl(studentId, enclosureId) {
    return `${API_BASE}/${studentId}/enclosures/${enclosureId}`;
}

