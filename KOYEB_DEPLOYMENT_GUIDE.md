# Koyeb Deployment Guide for CSD Project

## Prerequisites
1. GitHub account
2. Koyeb account (sign up at https://www.koyeb.com/)
3. Your code pushed to GitHub repository

## Deployment Steps

### 1. Push Your Code to GitHub
```powershell
# Initialize git (if not already done)
git init

# Add all files
git add .

# Commit changes
git commit -m "Prepare for Koyeb deployment"

# Add your GitHub repository as remote
git remote add origin https://github.com/PremPandey98/CSDApi.git

# Push to GitHub
git push -u origin main
```

### 2. Sign Up/Login to Koyeb
- Go to https://www.koyeb.com/
- Click "Sign Up" or "Login"
- You can sign up using GitHub (recommended)

### 3. Create a New Koyeb Service

#### a. Click "Create Service"
- In your Koyeb dashboard, click the "Create Service" button

#### b. Select Deployment Method
- Choose **"GitHub"** as your deployment method
- Authorize Koyeb to access your GitHub repositories
- Select your repository: `PremPandey98/CSDApi`
- Select branch: `main`

#### c. Configure Build Settings
- **Builder**: Docker
- **Dockerfile path**: `Dockerfile` (it will auto-detect)
- **Build context**: `/` (root directory)

#### d. Configure Service Settings
- **Service name**: csd-api (or your preferred name)
- **Region**: Choose closest to your users (e.g., Washington, Frankfurt)
- **Instance type**: 
  - Free tier: Eco (512MB RAM, 0.1 vCPU) - for testing
  - Production: Starter or higher (2GB RAM, 1 vCPU)

#### e. Configure Port
- **Port**: 8000 (this matches your Dockerfile EXPOSE directive)
- **Protocol**: HTTP

#### f. Add Environment Variables
Click "Add Environment Variable" and add each of these:

**Database:**
```
ConnectionStrings__DefaultConnection = Server=SQL1003.site4now.net;Initial Catalog=db_ac0a2d_csdpcb;User Id=db_ac0a2d_csdpcb_admin;Password=bcpm@100
```

**JWT Settings:**
```
CSDSetting__SecretKey = BCPM@100SecretKeyForJWTValidation2025!
CSDSetting__Issuer = csdapp
CSDSetting__Audience = csdapp
```

**Email Settings:**
```
EmailSettings__SmtpHost = smtp.gmail.com
EmailSettings__Username = bcpminnovation@gmail.com
EmailSettings__Password = jfan szea iqiy sqog
EmailSettings__FromEmail = bcpminnovation@gmail.com
```

**Cloudinary Settings:**
```
CloudinarySettings__CloudName = dq7eagyr9
CloudinarySettings__ApiKey = 989266186564121
CloudinarySettings__ApiSecret = Rb6N6yoPhkNO8Oi2Wc13tOzVjyE
```

**ASP.NET Core Settings:**
```
ASPNETCORE_ENVIRONMENT = Production
```

#### g. Health Check (Optional but Recommended)
- **Health check path**: `/` or create a dedicated endpoint like `/health`
- **Port**: 8000

#### h. Auto-Deploy
- Enable "Auto-deploy" to automatically deploy when you push to GitHub

### 4. Deploy
- Review all settings
- Click **"Deploy"**
- Wait 5-10 minutes for the first deployment

### 5. Monitor Deployment
- Watch the build logs in real-time
- Check for any errors
- Once deployed, you'll get a URL like: `https://csd-api-yourname.koyeb.app`

### 6. Test Your API
```powershell
# Test with curl or PowerShell
Invoke-RestMethod -Uri "https://csd-api-yourname.koyeb.app" -Method Get
```

## Troubleshooting

### Build Fails
- Check build logs in Koyeb dashboard
- Verify Dockerfile is correct
- Ensure all project files are committed to GitHub

### Application Doesn't Start
- Check runtime logs
- Verify environment variables are set correctly
- Ensure database connection string is accessible

### Port Issues
- Koyeb automatically sets PORT environment variable
- Your app is configured to use PORT or default to 8000

### Database Connection Issues
- Verify your SQL server allows connections from Koyeb IPs
- Check firewall rules on your database server
- Test connection string locally first

## Post-Deployment

### Update Your Frontend
Update your frontend API URL to point to your Koyeb URL:
```
https://csd-api-yourname.koyeb.app
```

### SSL Certificate
Koyeb automatically provides SSL/HTTPS for your application

### Scaling
- Go to your service settings
- Adjust instance size or add more instances
- Koyeb supports horizontal and vertical scaling

### Custom Domain (Optional)
1. Go to service settings
2. Click "Domains"
3. Add your custom domain
4. Update DNS records as instructed

### Monitoring
- View logs in real-time from Koyeb dashboard
- Monitor CPU, memory, and network usage
- Set up alerts for downtime

## Cost Estimation
- **Free Tier**: Limited resources, auto-sleeps after inactivity
- **Eco**: ~$0/month (with limitations)
- **Starter**: ~$7/month (2GB RAM, 1 vCPU)
- **Business**: ~$20/month+ (more resources)

## Auto-Deployment Workflow
Once set up, your workflow becomes:
```powershell
# 1. Make changes to code
# 2. Commit changes
git add .
git commit -m "Your changes"

# 3. Push to GitHub
git push

# 4. Koyeb automatically deploys! ✅
```

## Important Notes
1. **Sensitive Data**: Never commit `appsettings.json` with real credentials to public repos
2. **File Storage**: Use Cloudinary (already configured) for file uploads instead of local storage
3. **Database**: Ensure your database server accepts connections from internet
4. **Logs**: Check Koyeb logs regularly for any issues
5. **CORS**: Your app allows all origins - configure properly for production

## Support
- Koyeb Documentation: https://www.koyeb.com/docs
- Koyeb Community: https://community.koyeb.com/
- GitHub Issues: Create issues in your repository

---
**Happy Deploying! 🚀**
