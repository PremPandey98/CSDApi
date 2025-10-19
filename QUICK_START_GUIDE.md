# Quick Start Guide - Notice & Announcement API

## 🚀 **Ready to Test!**

Your Notice and Announcement API is fully implemented with **pagination**, **file uploads**, **authorization**, and **advanced filtering**.

---

## 📋 **Quick Test Checklist**

### **1. Start the Application**
```powershell
cd d:\CSDProject\API\CSD\CSDProject\CSDProject.API
dotnet run
```

---

## 🧪 **Test Scenarios**

### **Scenario 1: Create a Notice (Teacher/Admin)**

**Endpoint:** `POST /api/notice/create`

**Requirements:**
- Must be logged in as TEACHER or SUPER_ADMIN
- Use JWT token from login

**Test Data:**
```
Title: Semester Exam Schedule 2025
Content: Semester exams will begin from November 1st, 2025. All students must report 15 minutes before the exam time. Please carry your ID card and admit card.
Category: Academic
Priority: High
TargetAudience: Student
IsActive: true
IsPinned: true
ExpiryDate: 2025-12-31
Attachment: [Optional PDF file]
```

**Expected Result:**
- Status: 201 Created
- Returns notice with ID, full URL for attachment
- Creator name and role included

---

### **Scenario 2: Get All Notices with Pagination**

**Endpoint:** `GET /api/notice/all?pageNumber=1&pageSize=10`

**Expected Result:**
```json
{
  "data": [/* 10 notices */],
  "pageNumber": 1,
  "pageSize": 10,
  "totalCount": 45,
  "totalPages": 5,
  "hasPreviousPage": false,
  "hasNextPage": true
}
```

---

### **Scenario 3: Search Notices**

**Endpoint:** `GET /api/notice/all?search=exam&priority=High&pageNumber=1&pageSize=5`

**Expected Result:**
- Only notices containing "exam" in title or content
- Only High priority notices
- Maximum 5 results per page

---

### **Scenario 4: Get Active Notices (Public - No Auth)**

**Endpoint:** `GET /api/notice/active`

**Expected Result:**
- All active notices (IsActive = true)
- Not expired (ExpiryDate > today)
- No authentication required
- Pinned notices appear first

---

### **Scenario 5: Filter by Audience**

**Endpoint:** `GET /api/notice/by-audience/Student`

**Expected Result:**
- Notices where TargetAudience = "Student" OR "All"
- Active and non-expired only
- Public access (no auth)

---

### **Scenario 6: View Single Notice**

**Endpoint:** `GET /api/notice/1`

**Expected Result:**
- Full notice details
- Creator information
- View count incremented by 1
- Attachment URL if exists

---

### **Scenario 7: Update Notice (Teacher/Admin)**

**Endpoint:** `PUT /api/notice/update/1`

**Test:** Change priority from "Normal" to "Urgent"

**Expected Result:**
- Status: 200 OK
- UpdatedAt field populated
- Priority changed to "Urgent"

---

### **Scenario 8: Delete Notice (Teacher/Admin)**

**Endpoint:** `DELETE /api/notice/delete/1`

**Expected Result:**
- Status: 200 OK
- Notice marked as deleted (IsDeleted = true)
- Notice no longer appears in GET requests

---

### **Scenario 9: Advanced Filtering**

**Endpoint:**
```
GET /api/notice/all?
  category=Academic&
  priority=High&
  targetAudience=Student&
  isPinned=true&
  isActive=true&
  startDate=2025-01-01&
  endDate=2025-12-31&
  pageNumber=1&
  pageSize=20
```

**Expected Result:**
- Only notices matching ALL criteria
- Paginated results (20 per page)

---

### **Scenario 10: File Upload Test**

**Endpoint:** `POST /api/notice/create`

**Test Files:**
✅ Valid: exam_schedule.pdf (5MB)
✅ Valid: poster.jpg (2MB)
❌ Invalid: document.txt (not allowed)
❌ Invalid: large_file.pdf (15MB - exceeds limit)

**Expected Results:**
- Valid files: Uploaded successfully, URL returned
- Invalid type: Error "Invalid file type"
- Too large: Error "File size must be less than 10MB"

---

## 📊 **Pagination Test Cases**

### **Test Case 1: First Page**
```
GET /api/notice/all?pageNumber=1&pageSize=10
```
Expected:
- `hasPreviousPage: false`
- `hasNextPage: true` (if totalCount > 10)

### **Test Case 2: Middle Page**
```
GET /api/notice/all?pageNumber=3&pageSize=10
```
Expected:
- `hasPreviousPage: true`
- `hasNextPage: true` (if more pages exist)

### **Test Case 3: Last Page**
```
GET /api/notice/all?pageNumber=5&pageSize=10
```
Expected:
- `hasPreviousPage: true`
- `hasNextPage: false`

### **Test Case 4: Different Page Sizes**
```
GET /api/notice/all?pageSize=5   (5 items)
GET /api/notice/all?pageSize=20  (20 items)
GET /api/notice/all?pageSize=50  (50 items)
```

---

## 🔐 **Authorization Test Cases**

### **Test Case 1: Public Access (Should Work)**
- `GET /api/notice/active` ✅
- `GET /api/announcement/by-audience/Student` ✅

### **Test Case 2: Without Token (Should Fail - 401)**
- `POST /api/notice/create` ❌ Unauthorized
- `PUT /api/notice/update/1` ❌ Unauthorized
- `DELETE /api/notice/delete/1` ❌ Unauthorized

### **Test Case 3: Student Role (Should Fail - 403)**
Login as Student → Try:
- `POST /api/notice/create` ❌ Forbidden
- `DELETE /api/notice/delete/1` ❌ Forbidden

### **Test Case 4: Teacher Role (Should Work)**
Login as Teacher → Try:
- `POST /api/notice/create` ✅ Success
- `PUT /api/notice/update/1` ✅ Success
- `DELETE /api/notice/delete/1` ✅ Success

### **Test Case 5: Admin Role (Should Work)**
Login as SUPER_ADMIN → All operations ✅

---

## 🎯 **Sample Test Data**

### **Notice Examples:**

**1. Urgent Exam Notice**
```
Title: Final Exam Postponed
Content: Due to unforeseen circumstances, final exams have been postponed by 1 week. New dates will be announced soon.
Category: Academic
Priority: Urgent
TargetAudience: Student
IsPinned: true
```

**2. Event Announcement**
```
Title: Annual Tech Fest 2025
Content: Join us for the biggest tech event of the year! Exciting competitions, workshops, and prizes.
Category: Event
Priority: Normal
TargetAudience: All
IsPinned: false
ExpiryDate: 2025-12-20
```

**3. Faculty Meeting Notice**
```
Title: Department Meeting - Monday 10 AM
Content: All faculty members must attend the department meeting on Monday at 10 AM in Conference Hall.
Category: General
Priority: High
TargetAudience: Teacher
IsPinned: true
```

---

## 📱 **Frontend Integration Tips**

### **Display Pagination UI:**
```javascript
// Using the API response
const { data, pageNumber, totalPages, hasNextPage, hasPreviousPage } = response;

// Show page info
"Page {pageNumber} of {totalPages}"

// Enable/disable buttons
previousButton.disabled = !hasPreviousPage;
nextButton.disabled = !hasNextPage;
```

### **Show Pinned Notices First:**
```javascript
// API already sorts by IsPinned first, then by CreatedAt
// Just display in order received
notices.forEach(notice => {
  if (notice.isPinned) {
    // Show with special styling (e.g., yellow background)
  }
});
```

### **Handle File Downloads:**
```javascript
// attachmentUrl is full URL
<a href={notice.attachmentUrl} download>
  Download Attachment
</a>
```

---

## ✅ **Verification Checklist**

- [ ] Can create notice as Teacher/Admin
- [ ] Cannot create notice as Student
- [ ] Pagination works (page 1, 2, 3...)
- [ ] Search finds correct notices
- [ ] Filter by priority works
- [ ] Filter by audience works
- [ ] File upload works (PDF, JPG)
- [ ] Invalid file type rejected
- [ ] Large file (>10MB) rejected
- [ ] View count increments on GET by ID
- [ ] Pinned notices appear first
- [ ] Expired notices not shown in /active
- [ ] Soft delete works (notice hidden after delete)
- [ ] Update works and sets UpdatedAt
- [ ] Public endpoints work without auth
- [ ] Protected endpoints require auth

---

## 🐛 **Common Issues & Solutions**

### **Issue 1: "Unauthorized" on all requests**
**Solution:** Make sure you're sending JWT token in Authorization header:
```
Authorization: Bearer <your-jwt-token>
```

### **Issue 2: Pagination returns empty**
**Solution:** Check if you have data in database. Try pageNumber=1 first.

### **Issue 3: File upload fails**
**Solution:** 
- Check file size (<10MB)
- Check file type (PDF, JPG, PNG, DOC, DOCX only)
- Use `multipart/form-data` content type

### **Issue 4: "Invalid user token"**
**Solution:** Your JWT token might be expired or invalid. Login again.

### **Issue 5: Cannot find uploaded file**
**Solution:** Check that `wwwroot/uploads/notices/` folder exists. The API creates it automatically.

---

## 📝 **Database Tables Created**

### **csd_notices**
```sql
- notice_id (PK)
- title
- content
- category
- priority
- target_audience
- is_active
- is_pinned
- attachment_path
- view_count
- expiry_date
- created_by (FK to csd_user_registration)
- created_at
- updated_at
- is_deleted
```

### **csd_announcements**
(Same structure as notices with announcement_id as PK)

---

## 🎓 **Next Steps**

1. ✅ **Test all endpoints** using Postman or REST Client
2. ✅ **Verify pagination** with different page sizes
3. ✅ **Test file uploads** with various file types
4. ✅ **Check authorization** with different roles
5. ⏳ **Optional:** Implement email notifications (Task 18)
6. ⏳ **Optional:** Add background job for auto-expiry (Task 19)
7. ⏳ **Optional:** Write unit tests (Task 20)

---

## 🎉 **You're All Set!**

Your Notice and Announcement API is **fully functional** with:
✅ Pagination
✅ File Uploads
✅ Authorization
✅ Search & Filter
✅ Soft Delete
✅ View Tracking

**Happy Testing!** 🚀
